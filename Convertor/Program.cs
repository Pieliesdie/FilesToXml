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

public record ParsedFile(string Path, string Label, Encoding Encoding, SupportedFileExt? Type, string Delimiter, char[] SearchingDelimiters);
partial class Program
{
    private static XElement ProcessFile(ParsedFile file, TextWriter ErrorOut = null, TextWriter LogOut = null, bool isLog = false)
    {
        if (File.Exists(file.Path).Not())
        {
            ErrorOut?.WriteLine($"Input file {file.Path} doesn't exist");
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
            var xml = convertor switch
            {
                IDelimiterConvertable c when file.Delimiter == "auto" => c.ConvertByFile(file.Path, file.SearchingDelimiters, file.Encoding),
                IDelimiterConvertable c => c.ConvertByFile(file.Path, file.Delimiter, file.Encoding),
                IEncodingConvertable c => c.ConvertByFile(file.Path, file.Encoding),
                { } c => c.ConvertByFile(file.Path)
            };
            xml.Add(new XAttribute("ext", file.Type));
            xml.Add(new XAttribute("path", file.Path));
            if (file.Label is not null)
            {
                xml.Add(new XAttribute("label", file.Label));
            }
            if (isLog)
            {
                LogOut?.WriteLine($"Succesful convert file: {file.Path}");
            }
            return xml;
        }
        catch (Exception e)
        {
            ErrorOut?.WriteLine($"Failed convert file: {file.Path}, {e.Message}");
            return null;
        }
    }

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
            var xDoc = new XDocument(new XElement("DATA", datasets))
            {
                Declaration = new("1.0", Encoding.GetEncoding(args.OutputEncoding).WebName, null)
            };
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
                    Console.WriteLine($"Convert succesful all files to {args.Output}");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        });
    }
}
