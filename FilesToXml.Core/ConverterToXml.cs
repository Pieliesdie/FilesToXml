using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using FilesToXml.Core.Converters;
using FilesToXml.Core.Converters.Interfaces;

namespace FilesToXml.Core;
public static class ConverterToXml
{
    public static bool Convert(IOptions options, TextWriter outputWritter, TextWriter errorWritter)
    {
        if (options is { Output: not null, ForceSave: false } && File.Exists(options.Output))
        {
            errorWritter.WriteLine("Output file already exist and ForceSave is false");
            return false;
        }
        options.Input = Extensions.UnpackFolders(options.Input).ToArray();
        Queue<string> delimeters = new(options.Delimiters);

        var files = options.Input.Select((filePath, index) => new FileInformation(
            path: filePath.RelativePathToAbsoluteIfNeed(),
            label: (options.Labels?.Any() ?? false) ? options.Labels.ElementAtOrDefault(index) : null,
            encoding: Encoding.GetEncoding(index > options.InputEncoding.Count() - 1 ? options.InputEncoding.Last() : options.InputEncoding.ElementAt(index)),
            type: filePath.GetExtFromPath(),
            delimiter: filePath.GetDelimiter(delimeters),
            searchingDelimiters: options.SearchingDelimiters?.ToArray() ?? [';', '|', '\t', ',']
        )).ToList();

        var datasets = files
            .AsParallel()
            .AsUnordered()
            .Select(file => ProcessFile(file, errorWritter, outputWritter, options.Output != null))
            .ToList();

        var xDoc = new XStreamingElement("DATA", datasets);
        try
        {
            var isPrintToOutputWriter = string.IsNullOrWhiteSpace(options.Output);
            var saveOptions = options.DisableFormat ? SaveOptions.DisableFormatting : SaveOptions.None;
            if (isPrintToOutputWriter)
            {
                xDoc.Save(outputWritter, saveOptions);
            }
            else
            {
                using var sw = new StreamWriter(options.Output!, false, Encoding.GetEncoding(options.OutputEncoding));
                xDoc.Save(sw, saveOptions);
                outputWritter.WriteLine(datasets.Any(x => x is null)
                    ? "Converted all files with some errors"
                    : $"Converted succesful all files to {options.Output}");
            }
            files.ForEach(file => file.Dispose());
        }
        catch (Exception e)
        {
            errorWritter.WriteLine($"Failed to convert result document: {e.Message}");
            return false;
        }
        return true;
    }

    public static XStreamingElement? Convert(
        FileInformation fileInformation,
        TextWriter? errorWriter = null,
        TextWriter? logWriter = null,
        bool showLog = false)
    {
        return ProcessFile(fileInformation, errorWriter, logWriter, showLog);
    }

    private static XStreamingElement? ProcessFile(FileInformation fileInformation, TextWriter? errorWriter = null, TextWriter? logWriter = null, bool showLog = false)
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
                _ => throw new NotImplementedException($"Unsupported type")
            };

            var additionalInfo = new List<XObject> { new XAttribute("ext", fileInformation.Type), new XAttribute("path", fileInformation.Path) };
            if (fileInformation.Label is not null)
            {
                additionalInfo.Add(new XAttribute("label", fileInformation.Label));
            }

            var stream = fileInformation.Stream;
            var xml = convertor switch
            {
                IDelimiterConvertable c when fileInformation.Delimiter == "auto" => c.Convert(stream, fileInformation.SearchingDelimiters, fileInformation.Encoding, additionalInfo),
                IDelimiterConvertable c => c.Convert(stream, fileInformation.Delimiter, fileInformation.Encoding, additionalInfo),
                IEncodingConvertable c => c.Convert(stream, fileInformation.Encoding, additionalInfo),
                { } c => c.Convert(stream, additionalInfo)
            };
            if (showLog)
            {
                logWriter?.WriteLine($"Succesful start converting file: {fileInformation.Path}");
            }
            return xml;
        }
        catch (Exception e)
        {
            errorWriter?.WriteLine($"Failed convert file: {fileInformation.Path}, {e.Message}");
            return null;
        }
    }
}
