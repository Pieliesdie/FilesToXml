using System;
using System.Collections.Generic;
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
    public static bool Convert(IOptions options, Stream outputStream, Stream errorStream)
    {
        var encoding = Encoding.GetEncoding(options.OutputEncoding);
        var outputPath = options.Output?.RelativePathToAbsoluteIfNeed();

        using var errorSw = CreateDefaulStreamWriter(errorStream, encoding);
        if (options is {Output: not null, ForceSave: false} && File.Exists(outputPath))
        {
            errorSw?.WriteLine("Output file already exist and ForceSave is false");
            return false;
        }

        var writeResultToStream = string.IsNullOrWhiteSpace(outputPath);
        using var outputSw = writeResultToStream
            ? CreateDefaulStreamWriter(outputStream, encoding)
            : CreateDefaulStreamWriter(outputPath!, encoding);
        using var logSw = writeResultToStream
            ? CreateDefaulStreamWriter(Stream.Null, encoding)
            : CreateDefaulStreamWriter(outputStream, encoding);

        options.Input = options.Input.UnpackFolders().ToArray();
        Queue<string> delimeters = new(options.Delimiters);

        List<FileInformation> files = options.Input.Select((filePath, index) => new FileInformation(
            filePath.RelativePathToAbsoluteIfNeed(),
            options.Labels?.ElementAtOrDefault(index),
            Encoding.GetEncoding(options.InputEncoding.ToList().ElementAtOrLast(index)),
            filePath.GetExtFromPath(),
            PopOrPeekDelimiter(filePath, delimeters),
            options.SearchingDelimiters?.ToArray() ?? [';', '|', '\t', ',']
        )).ToList();

        List<XStreamingElement?>? datasets = files
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
            logSw.WriteLine(datasets.Any(x => x is null)
                ? "Converted all files with some errors"
                : $"Converted succesful all files to {outputPath}");
            files.ForEach(file => file.Dispose());
            ResetStream(outputSw, logSw, errorSw);
        }
        catch (Exception e)
        {
            errorSw?.WriteLine($"Failed to convert result document: {e.Message}");
            return false;
        }

        return true;

        static string PopOrPeekDelimiter(string path, Queue<string> delimiters)
        {
            if (path.GetExtFromPath() == SupportedFileExt.Csv) return delimiters.Count > 1 ? delimiters.Dequeue() : delimiters.Peek();

            return ";";
        }
    }
    public static XStreamingElement? Convert(FileInformation fileInformation, StreamWriter? errorWriter = null, StreamWriter? logWriter = null)
    {
        return ProcessFile(fileInformation, errorWriter, logWriter);
    }
    private static XStreamingElement? ProcessFile(FileInformation fileInformation, TextWriter? errorWriter = null, TextWriter? logWriter = null)
    {
        if (!File.Exists(fileInformation.Path))
        {
            errorWriter?.WriteLine($"Input file {fileInformation.Path} doesn't exist");
            return null;
        }

        try
        {
            IConvertable convertor = fileInformation.Type switch
            {
                SupportedFileExt.Xls => new XlsToXml(),
                SupportedFileExt.Xlsx => new XlsxToXml(),
                SupportedFileExt.Txt => new TxtToXml(),
                SupportedFileExt.Csv => new CsvToXml(),
                SupportedFileExt.Docx => new DocxToXml(),
                SupportedFileExt.Doc => new DocToXml(),
                SupportedFileExt.Xml => new XmlToXml(),
                SupportedFileExt.Json => new JsonToXml(),
                SupportedFileExt.Tsv => new TsvToXml(),
                SupportedFileExt.Dbf => new DbfToXml(),
                /*SupportedFileExt.rtf => new RtfToXml(),
                SupportedFileExt.odt => new OdsToXml(),
                SupportedFileExt.ods => new OdsToXml(),*/
                _ => throw new NotImplementedException("Unsupported type")
            };

            var additionalInfo = new List<XObject>
            {
                new XAttribute("ext", fileInformation.Type!.ToString()!.ToLower()),
                new XAttribute("path", fileInformation.Path),
                new XAttribute("filename", Path.GetFileName(fileInformation.Path))
            };
            if (!string.IsNullOrEmpty(fileInformation.Label)) additionalInfo.Add(new XAttribute("label", fileInformation.Label));

            var stream = fileInformation.Stream;
            var xml = convertor switch
            {
                IDelimiterConvertable c when fileInformation.Delimiter == "auto" => c.Convert(stream,
                    fileInformation.SearchingDelimiters, fileInformation.Encoding, additionalInfo),
                IDelimiterConvertable c => c.Convert(stream, fileInformation.Delimiter, fileInformation.Encoding,
                    additionalInfo),
                IEncodingConvertable c => c.Convert(stream, fileInformation.Encoding, additionalInfo),
                { } c => c.Convert(stream, additionalInfo)
            };

            logWriter?.WriteLine($"Succesful start converting file: {fileInformation.Path}");
            return xml;
        }
        catch (Exception e)
        {
            errorWriter?.WriteLine($"Failed convert file: {fileInformation.Path}, {e.Message}");
            return null;
        }
    }
    private static StreamWriter CreateDefaulStreamWriter(string path, Encoding encoding)
    {
        return new StreamWriter(path, false, encoding)
            {AutoFlush = true};
    }
    private static StreamWriter CreateDefaulStreamWriter(Stream stream, Encoding encoding)
    {
        return new StreamWriter(stream, encoding, -1, true)
            {AutoFlush = true};
    }
    private static void ResetStream(params StreamWriter[] streams)
    {
        foreach (var stream in streams)
            if (stream.BaseStream.CanSeek)
                stream.BaseStream.Position = 0;
    }
}