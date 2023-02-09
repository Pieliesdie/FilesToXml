using ConverterToXml.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConverterToXml;
public static class ConverterToXml
{
    public static bool Convert(IOptions options, TextWriter outputWritter, TextWriter errorWritter)
    {
        if (options.Output != null && options.ForceSave.Not() && File.Exists(options.Output))
        {
            errorWritter.WriteLine("Output file already exist and ForceSave is false");
            return false;
        }
        options.Input = Extensions.UnpackFolders(options.Input).ToArray();
        Queue<string> delimeters = new(options.Delimiters);
        var files = options.Input.Select((filePath, index) => new ParsedFile(
            path: filePath.RelativePathToAbsoluteIfNeed(),
            label: (options.Labels?.Any() ?? false) ? options.Labels.ElementAtOrDefault(index) : null,
            encoding: Encoding.GetEncoding(index > options.InputEncoding.Count() - 1 ? options.InputEncoding.Last() : options.InputEncoding.ElementAt(index)),
            type: filePath.GetExtFromPath(),
            delimiter: filePath.GetDelimiter(delimeters),
            searchingDelimiters: options.SearchingDelimiters?.ToArray() ?? new[] { ';', '|', '\t', ',' }
        ));

        var datasets = files.AsParallel().AsUnordered().Select(file => ProcessFile(file, errorWritter, outputWritter, options.Output != null)).ToList();
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
                using var sw = new StreamWriter(options.Output, false, Encoding.GetEncoding(options.OutputEncoding));
                xDoc.Save(sw, saveOptions);
                if (datasets.Any(x => x is null))
                {
                    outputWritter.WriteLine($"Converted all files with some errors");
                }
                else
                {
                    outputWritter.WriteLine($"Converted succesful all files to {options.Output}");
                }
            }
        }
        catch (Exception e)
        {
            errorWritter.WriteLine($"Failed to convert result document: {e.Message}");
            return false;
        }
        return true;
    }
    private static XStreamingElement? ProcessFile(ParsedFile file, TextWriter? errorWriter = null, TextWriter? logWriter = null, bool showLog = false)
    {
        if (File.Exists(file.Path).Not())
        {
            errorWriter?.WriteLine($"Input file {file.Path} doesn't exist");
            return null;
        }
        try
        {
            IConvertable convertor = file.Type switch
            {
                SupportedFileExt.xls => new XlsToXml(),
                SupportedFileExt.xlsx => new XlsxToXml(),
                SupportedFileExt.txt => new TxtToXml(),
                SupportedFileExt.csv => new CsvToXml(),
                SupportedFileExt.docx => new DocxToXml(),
                SupportedFileExt.doc => new DocToXml(),
                SupportedFileExt.xml => new XmlToXml(),
                SupportedFileExt.json => new JsonToXml(),
                SupportedFileExt.tsv => new TsvToXml(),
                SupportedFileExt.dbf => new DbfToXml(),
                /*SupportedFileExt.rtf => new RtfToXml(),
                SupportedFileExt.odt => new OdsToXml(),
                SupportedFileExt.ods => new OdsToXml(),*/
                _ => throw new NotImplementedException($"Unsupported type")
            };

            var additionalInfo = new List<XObject> { new XAttribute("ext", file.Type), new XAttribute("path", file.Path) };
            if (file.Label is not null)
            {
                additionalInfo.Add(new XAttribute("label", file.Label));
            }
            // Do i need to dispose it, if simplier just close application?
            var stream = File.OpenRead(file.Path);
            var xml = convertor switch
            {
                IDelimiterConvertable c when file.Delimiter == "auto" => c.Convert(stream, file.SearchingDelimiters, file.Encoding, additionalInfo),
                IDelimiterConvertable c => c.Convert(stream, file.Delimiter, file.Encoding, additionalInfo),
                IEncodingConvertable c => c.Convert(stream, file.Encoding, additionalInfo),
                { } c => c.Convert(stream, additionalInfo)
            };
            if (showLog)
            {
                logWriter?.WriteLine($"Succesful start converting file: {file.Path}");
            }
            return xml;
        }
        catch (Exception e)
        {
            errorWriter?.WriteLine($"Failed convert file: {file.Path}, {e.Message}");
            return null;
        }
    }
}
