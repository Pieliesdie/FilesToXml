using ConverterToXml.Converters;
using System;
using System.IO;
using System.Text;
using System.Linq;
using CommandLine;
using System.Xml.Linq;
using ConverterToXml;
using System.Collections.Generic;

namespace Convertor;

partial class Program
{
    record ParsedFile(string Path, string Label, Encoding Encoding, SupportedFileExt? Type, string Delimiter, char[] SearchingDelimiters);
    static void Main(string[] args)
    {
        var isSupportCommand = args.FirstOrDefault(x => x == "--support") is not null;
        if (isSupportCommand)
        {
            var supportedTypes = Enum.GetNames(typeof(SupportedFileExt)).Aggregate((x, y) => $"{x}, {y}");
            Console.WriteLine($"Supported types: {supportedTypes}");
            return;
        }
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Parser.Default.ParseArguments<Options>(args).WithParsed(args =>
        {
            if (args.Labels.Any() && args.Input.Count() != args.Labels.Count())
            {
                Console.Error.WriteLine("Label count doesn't match the count of input files");
                return;
            }
            if (args.Output != null && args.ForceSave.Not() && File.Exists(args.Output))
            {
                Console.Error.WriteLine("Output file already exist and ForceSave is false");
                return;
            }

            Queue<string> delimeters = new(args.Delimiters);
            var files = args.Input.Select((filePath, index) => new ParsedFile(
                Path: filePath,
                Label: args.Labels.Any() ? args.Labels.ElementAt(index) : null,
                Encoding: Encoding.GetEncoding(index > args.InputEncoding.Count() - 1 ? args.InputEncoding.Last() : args.InputEncoding.ElementAt(index)),
                Type: filePath.GetExtFromPath(),
                Delimiter: filePath.GetDelimiter(delimeters),
                SearchingDelimiters: args.SearchingDelimiters?.ToArray()
            ));

            var datasets = files.AsParallel().Select(file => ProcessFile(file, Console.Error, Console.Out, args.Output != null)).ToArray();
            var xDoc = new XStreamingElement("DATA", datasets);
            try
            {
                if (args.Output == null)
                {
                    xDoc.Save(Console.Out, args.DisableFormat ? SaveOptions.DisableFormatting : SaveOptions.None);
                }
                else
                {
                    using var sw = new StreamWriter(args.Output, false, Encoding.GetEncoding(args.OutputEncoding));
                    xDoc.Save(sw, args.DisableFormat ? SaveOptions.DisableFormatting : SaveOptions.None);
                    if (datasets.Any(x => x is null))
                    {
                        Console.WriteLine($"Convert all files with some errors");
                    }
                    else
                    {
                        Console.WriteLine($"Convert succesful all files to {args.Output}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Failed to convert result document: {e.Message}");
            }
        });
    }

    private static XStreamingElement ProcessFile(ParsedFile file, TextWriter errorReader = null, TextWriter logReader = null, bool showLog = false)
    {
        if (File.Exists(file.Path).Not())
        {
            errorReader?.WriteLine($"Input file {file.Path} doesn't exist");
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
            // Do i need to dispose it, if faster just close application?
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
                logReader?.WriteLine($"Succesful convert file: {file.Path}");
            }                   
            return xml;
        }
        catch (Exception e)
        {
            errorReader?.WriteLine($"Failed convert file: {file.Path}, {e.Message}");
            return null;
        }
    }
}
