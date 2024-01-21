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
using EncodingExtensions = FilesToXml.Core.Extensions.EncodingExtensions;

namespace FilesToXml.Core;

public static class ConverterToXml
{
    public static XStreamingElement? Convert(FileInformation fileInformation, StreamWriter? errorWriter = null, StreamWriter? logWriter = null)
    {
        return ProcessFile(fileInformation, errorWriter ?? StreamWriter.Null, logWriter ?? StreamWriter.Null);
    }
    public static bool Convert(IOptions options, Stream outputStream, Stream errorStream)
    {
        if (!EncodingExtensions.TryGetEncoding(options.OutputEncoding, out var encoding, out var encodingError))
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
            errorSw.WriteLine("Output file already exists, and ForceSave is false");
            return false;
        }

        var writeResultToStream = string.IsNullOrWhiteSpace(outputPath);
        using var outputSw = writeResultToStream
            ? CreateDefaulStreamWriter(outputStream, encoding)
            : CreateDefaulStreamWriter(outputPath!, encoding);
        using var logSw = writeResultToStream
            ? CreateDefaulStreamWriter(Stream.Null, encoding)
            : CreateDefaulStreamWriter(outputStream, encoding);

        return ParseAndSave(options, outputSw, errorSw, logSw);
    }
    private static bool ParseAndSave(IOptions options, StreamWriter outputSw, StreamWriter errorSw, StreamWriter logSw)
    {
        try
        {
            List<FileInformation> files = ParseOptions(options, errorSw, out var hasErrors);
            List<XStreamingElement?> datasets = Convert(files, errorSw, logSw);
            var xDoc = new XStreamingElement("DATA", datasets);
            var saveOptions = options.DisableFormat ? SaveOptions.DisableFormatting : SaveOptions.None;
            xDoc.Save(outputSw, saveOptions);
            files.ForEach(file => file.Dispose());
            StreamExtensions.ResetStream(outputSw, logSw, errorSw);
            var outputPath = options.Output?.RelativePathToAbsoluteIfNeed();
            logSw.WriteLine(datasets.Any(x => x is null) || hasErrors
                ? "Converted all files with some errors"
                : $"Successfully converted all files to {outputPath}");
        }
        catch (Exception e)
        {
            errorSw.WriteLine($"Failed to convert result document: {e.Message}");
            return false;
        }

        return true;
    }
    private static List<XStreamingElement?> Convert(IEnumerable<FileInformation> filesInformation, TextWriter errorWriter, TextWriter logWriter)
    {
        return filesInformation
            .AsParallel()
            .AsUnordered()
            .Select(file => ProcessFile(file, errorWriter, logWriter))
            .ToList();
    }
    private static List<FileInformation> ParseOptions(IOptions options, TextWriter errorSw, out bool hasErrors)
    {
        hasErrors = false;
        var delimiters = new Queue<string>(options.Delimiters.DefaultIfEmpty(Defaults.Delimiter));
        List<FileInformation> files = [];
        var index = 0;
        foreach (var inputPath in options.Input.UnpackFolders())
        {
            index++;
            if (!TryParsePath(inputPath, errorSw, out var parsedPath))
            {
                hasErrors = true;
                continue;
            }

            var codepage = options.InputEncoding.ToList().ElementAtOrLast(index);
            if (!EncodingExtensions.TryGetEncoding(codepage, out var encoding, out var error))
            {
                hasErrors = true;
                errorSw.WriteLine($"'{parsedPath.Filename}': {error}");
                errorSw.WriteLine($"Using default encoding for '{parsedPath.Filename}': {Defaults.Encoding.CodePage} ({Defaults.Encoding.WebName})");
                encoding = Defaults.Encoding;
            }

            var fileInfo = new FileInformation
            {
                Path = inputPath,
                Name = parsedPath.Filename,
                Stream = parsedPath.Stream,
                Encoding = encoding,
                Type = inputPath.ToFiletype(),
                Label = options.Labels?.ElementAtOrDefault(index)
            };
            if (parsedPath.Filetype == Filetype.Csv)
            {
                fileInfo.Delimiter = delimiters.Count > 1 ? delimiters.Dequeue() : delimiters.Peek();
                fileInfo.SearchingDelimiters = options.SearchingDelimiters?.ToArray() ?? Defaults.SearchingDelimiters;
            }

            files.Add(fileInfo);
        }

        return files;
    }
    private static bool TryParsePath(string inputPath, TextWriter errorWriter, [NotNullWhen(true)] out ParsePathResult? parsePathResult)
    {
        parsePathResult = null;
        if (!File.Exists(inputPath))
        {
            errorWriter.WriteLine($"Input file {inputPath} doesn't exist");
            return false;
        }

        var filename = Path.GetFileName(inputPath);
        var fileType = inputPath.ToFiletype();
        Stream stream;
        try
        {
            stream = File.OpenRead(inputPath.RelativePathToAbsoluteIfNeed());
        }
        catch (Exception ex)
        {
            errorWriter.WriteLine($"'{filename}': {ex.Message}");
            return false;
        }

        parsePathResult = new ParsePathResult(filename, stream, fileType);
        return true;
    }
    private static XStreamingElement? ProcessFile(FileInformation fileInfo, TextWriter errorWriter, TextWriter logWriter)
    {
        try
        {
            var converter = FindConverter(fileInfo.Type);
            var additionalInfo = CreateAdditionalInfo(fileInfo);
            var xml = converter switch
            {
                IDelimiterConvertable c when fileInfo.IsAutoDelimiter
                    => c.Convert(fileInfo.Stream, fileInfo.SearchingDelimiters ?? Defaults.SearchingDelimiters, fileInfo.Encoding, additionalInfo),
                IDelimiterConvertable c => c.Convert(fileInfo.Stream, fileInfo.Delimiter ?? Defaults.Delimiter, fileInfo.Encoding, additionalInfo),
                IEncodingConvertable c => c.Convert(fileInfo.Stream, fileInfo.Encoding, additionalInfo),
                _ => converter.Convert(fileInfo.Stream, additionalInfo)
            };

            logWriter.WriteLine($"Successfully start converting file: {fileInfo.Name}");
            return xml;
        }
        catch (Exception e)
        {
            errorWriter.WriteLine($"Failed to convert file: {fileInfo.Name}, {e.Message}");
            return null;
        }
    }
    private static List<XObject> CreateAdditionalInfo(FileInformation fileInfo)
    {
        var additionalInfo = new List<XObject>
        {
            new XAttribute("ext", fileInfo.Type.ToString().ToLower()),
            new XAttribute("name", fileInfo.Name)
        };
        if (!string.IsNullOrEmpty(fileInfo.Path))
            additionalInfo.Add(new XAttribute("path", fileInfo.Path));

        if (!string.IsNullOrEmpty(fileInfo.Label))
            additionalInfo.Add(new XAttribute("label", fileInfo.Label));
        return additionalInfo;
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
            Filetype.Unknown => throw new NotSupportedException("Unsupported type"),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
        return converter;
    }
    private static StreamWriter CreateDefaulStreamWriter(string path, Encoding encoding)
    {
        return new StreamWriter(path, false, encoding) {AutoFlush = true};
    }
    private static StreamWriter CreateDefaulStreamWriter(Stream stream, Encoding encoding)
    {
        return new StreamWriter(stream, encoding, -1, true) {AutoFlush = true};
    }

    private record ParsePathResult(string Filename, Stream Stream, Filetype Filetype);
}