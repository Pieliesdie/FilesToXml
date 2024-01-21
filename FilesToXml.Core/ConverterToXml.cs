using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Extensions;

namespace FilesToXml.Core;

public static class ConverterToXml
{
    public static XStreamingElement? Convert(FileInformation fileInformation, StreamWriter? errorWriter = null, StreamWriter? logWriter = null)
    {
        return ProcessFile(fileInformation, errorWriter, logWriter);
    }
    public static bool Convert(IOptions options, Stream outputStream, Stream errorStream)
    {
        if (!TryGetEncoding(options.OutputEncoding, out var encoding, out var encodingError))
        {
            using var fallBackErrorSw = CreateDefaulStreamWriter(errorStream, Defaults.Encoding);
            fallBackErrorSw.WriteLine($"{encodingError}");
            fallBackErrorSw.WriteLine($"Using default encoding for output: {Defaults.Encoding.CodePage} ({Defaults.Encoding.WebName})");
            return false;
        }

        var outputPath = options.Output?.RelativePathToAbsoluteIfNeed();

        using var errorSw = CreateDefaulStreamWriter(errorStream, encoding);
        if (options is {Output: not null, ForceSave: false} && File.Exists(outputPath))
        {
            errorSw.WriteLine("Output file already exist and ForceSave is false");
            return false;
        }

        var writeResultToStream = string.IsNullOrWhiteSpace(outputPath);
        using var outputSw = writeResultToStream
            ? CreateDefaulStreamWriter(outputStream, encoding)
            : CreateDefaulStreamWriter(outputPath!, encoding);
        using var logSw = writeResultToStream
            ? CreateDefaulStreamWriter(Stream.Null, encoding)
            : CreateDefaulStreamWriter(outputStream, encoding);

        List<FileInformation> files = ExtractInfo(options, errorSw, out var hasErrors);
        List<XStreamingElement?> datasets = files
            .AsParallel()
            .AsUnordered()
            // ReSharper disable AccessToDisposedClosure
            .Select(file => ProcessFile(file, errorSw, logSw))
            .ToList();

        var xDoc = new XStreamingElement("DATA", datasets);
        try
        {
            var saveOptions = options.DisableFormat ? SaveOptions.DisableFormatting : SaveOptions.None;
            xDoc.Save(outputSw, saveOptions);
            logSw.WriteLine(datasets.Any(x => x is null) || hasErrors
                ? "Converted all files with some errors"
                : $"Converted succesful all files to {outputPath}");
            files.ForEach(file => file.Dispose());
            ResetStream(outputSw, logSw, errorSw);
        }
        catch (Exception e)
        {
            errorSw.WriteLine($"Failed to convert result document: {e.Message}");
            return false;
        }

        return true;
    }
    private static bool TryGetEncoding(int codepage, [NotNullWhen(true)] out Encoding? encoding, out string error)
    {
        encoding = Defaults.Encoding;
        error = string.Empty;
        try
        {
            encoding = Encoding.GetEncoding(codepage);
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
    private static List<FileInformation> ExtractInfo(IOptions options, TextWriter errorSw, out bool hasErrors)
    {
        hasErrors = false;
        var input = options.Input.UnpackFolders().ToArray();
        Queue<string> delimiters = new(options.Delimiters.DefaultIfEmpty(Defaults.Delimiter));
        List<FileInformation> files = [];
        var index = 0;
        foreach (var inputPath in input)
        {
            index++;
            if (!File.Exists(inputPath))
            {
                errorSw.WriteLine($"Input file {inputPath} doesn't exist");
                hasErrors = true;
                continue;
            }

            var filename = Path.GetFileName(inputPath);
            var fileType = inputPath.ToSupportedFile();
            if (!TryGetEncoding(options.InputEncoding.ToList().ElementAtOrLast(index), out var encoding, out var error))
            {
                errorSw.WriteLine($"'{filename}': {error}");
                errorSw.WriteLine($"Using default encoding for '{filename}': {Defaults.Encoding.CodePage} ({Defaults.Encoding.WebName})");
                encoding = Defaults.Encoding;
            }

            Stream stream;
            try
            {
                stream = File.OpenRead(inputPath.RelativePathToAbsoluteIfNeed());
            }
            catch(Exception ex)
            {
                errorSw.WriteLine($"'{filename}': {ex.Message}");
                continue;
            }

            var fileInfo = new FileInformation()
            {
                Path = inputPath,
                Name = filename,
                Stream = stream,
                Encoding = encoding,
                Type = inputPath.ToSupportedFile(),
                Label = options.Labels?.ElementAtOrDefault(index)
            };
            if (fileType == Filetype.Csv)
            {
                fileInfo.Delimiter = delimiters.Count > 1 ? delimiters.Dequeue() : delimiters.Peek();
                fileInfo.SearchingDelimiters = options.SearchingDelimiters?.ToArray() ?? Defaults.SearchingDelimiters;
            }

            files.Add(fileInfo);
        }

        return files;
    }
    private static XStreamingElement? ProcessFile(FileInformation fileInformation, TextWriter? errorWriter = null, TextWriter? logWriter = null)
    {
        try
        {
            IConvertable converter = FindConverter(fileInformation.Type);
            var additionalInfo = new List<XObject>
            {
                new XAttribute("ext", fileInformation.Type.ToString()!.ToLower()),
                new XAttribute("name", fileInformation.Name),
            };
            if (!string.IsNullOrEmpty(fileInformation.Path))
                additionalInfo.Add(new XAttribute("path", fileInformation.Path));

            if (!string.IsNullOrEmpty(fileInformation.Label))
                additionalInfo.Add(new XAttribute("label", fileInformation.Label));

            var stream = fileInformation.Stream;
            var xml = converter switch
            {
                IDelimiterConvertable c when fileInformation.Delimiter == "auto" => c.Convert(stream, fileInformation.SearchingDelimiters ?? Defaults.SearchingDelimiters, fileInformation.Encoding, additionalInfo),
                IDelimiterConvertable c => c.Convert(stream, fileInformation.Delimiter ?? Defaults.Delimiter, fileInformation.Encoding, additionalInfo),
                IEncodingConvertable c => c.Convert(stream, fileInformation.Encoding, additionalInfo),
                _ => converter.Convert(stream, additionalInfo)
            };

            logWriter?.WriteLine($"Succesful start converting file: {fileInformation.Name}");
            return xml;
        }
        catch (Exception e)
        {
            errorWriter?.WriteLine($"Failed convert file: {fileInformation.Name}, {e.Message}");
            return null;
        }
    }
    private static IConvertable FindConverter(Filetype type)
    {
        IConvertable converter = type switch
        {
            Filetype.Xls => new XlsToXml(),
            Filetype.Xlsx => new XlsxToXml(),
            Filetype.Txt => new TxtToXml(),
            Filetype.Csv => new CsvToXml(),
            Filetype.Docx => new DocxToXml(),
            Filetype.Doc => new DocToXml(),
            Filetype.Xml => new XmlToXml(),
            Filetype.Json => new JsonToXml(),
            Filetype.Tsv => new TsvToXml(),
            Filetype.Dbf => new DbfToXml(),
            /*SupportedFileExt.rtf => new RtfToXml(),
            SupportedFileExt.odt => new OdsToXml(),
            SupportedFileExt.ods => new OdsToXml(),*/
            Filetype.Unknown or _ => throw new NotImplementedException("Unsupported type")
        };
        return converter;
    }
    private static StreamWriter CreateDefaulStreamWriter(string path, Encoding encoding) => new(path, false, encoding) {AutoFlush = true};
    private static StreamWriter CreateDefaulStreamWriter(Stream stream, Encoding encoding) => new(stream, encoding, -1, true) {AutoFlush = true};
    private static void ResetStream(params StreamWriter[] streams)
    {
        foreach (var stream in streams)
            if (stream.BaseStream.CanSeek)
                stream.BaseStream.Position = 0;
    }
}