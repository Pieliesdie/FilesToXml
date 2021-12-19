using ConverterToXml.Converters;
using System;
using System.IO;
using System.Text;
using System.Linq;
using CommandLine;
using System.Xml.Linq;
using ConverterToXml;
using System.Collections;
using System.Collections.Generic;

namespace Convertor
{

    public record ParsedFile(string Path, string Label, Encoding Encoding, SupportedFileExt? Type, string Delimiter);
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
                    IDelimiterConvertable c => c.ConvertByFile(file.Path, file.Delimiter, file.Encoding),
                    IEncodingConvertable c => c.ConvertByFile(file.Path, file.Encoding),
                    IConvertable c => c.ConvertByFile(file.Path)
                };
                xml.Root.Add(new XAttribute("ext", file.Type));
                xml.Root.Add(new XAttribute("path", file.Path));
                if (file.Label is not null)
                {
                    xml.Root.Add(new XAttribute("label", file.Label));
                }
                if (isLog)
                {
                    LogOut?.WriteLine($"Succesful convert file: {file.Path}");
                }
                GC.Collect();
                return xml.Root;
            }
            catch (Exception e)
            {
                ErrorOut?.WriteLine($"Failed convert file: {file.Path}, {e.Message}");
                return null;
            }
        }

        static void Main(string[] args)
        {
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
            var isSupportCommand = args.FirstOrDefault(x => x == "--support") is not null;
            if (isSupportCommand)
            {
                var supportedTypes = Enum.GetNames(typeof(SupportedFileExt)).Aggregate((x, y) => $"{x}, {y}");
                Console.WriteLine($"Supported types: {supportedTypes}");
                return;
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(args =>
                {
                    if (args.Labels.Count() > 0 && args.Input.Count() != args.Labels.Count())
                    {
                        Console.Error.WriteLine("Label count doesn't match the count of input files");
                        return;
                    }
                    if (args.Output != null && args.ForceSave.Not() && File.Exists(args.Output))
                    {
                        Console.Error.WriteLine("Output file already exist and ForceSave is false");
                        return;
                    }

                    var XDoc = new XDocument(new XElement("DATA"));
                    XDoc.Declaration = new XDeclaration("1.0", Encoding.GetEncoding(args.OutputEncoding).WebName, null);

                    Queue<string> delimeters = new Queue<string>(args.Delimiters);
                    var files = args.Input.Select((filePath, index) => new ParsedFile(
                        Path: filePath,
                        Label: args.Labels.Count() > 0 ? args.Labels.ElementAt(index) : null,
                        Encoding: Encoding.GetEncoding(index > args.InputEncoding.Count() - 1 ? args.InputEncoding.Last() : args.InputEncoding.ElementAt(index)),
                        Type: filePath.GetExtFromPath(),
                        Delimiter: filePath.GetDelimiter(delimeters)
                    ));
                    XDoc.Root.Add(files.AsParallel().Select(file => ProcessFile(file, Console.Error, Console.Out, args.Output != null)).ToArray());
                    try
                    {
                        if (args.Output == null)
                        {
                            Console.WriteLine(XDoc.ToStringWithDeclaration());
                        }
                        else
                        {
                            File.WriteAllText(args.Output, XDoc.ToStringWithDeclaration(), Encoding.GetEncoding(args.OutputEncoding));
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
}
