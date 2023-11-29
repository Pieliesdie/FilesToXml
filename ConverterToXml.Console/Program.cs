using CommandLine;
using ConverterToXml;
using System;
using System.Linq;
using System.Text;
using ConverterToXml.Console;
using ConverterToXml.Core;

var isSupportCommand = args.FirstOrDefault(x => x == "--support") is not null;
if (isSupportCommand)
{
    var supportedTypes = Enum.GetNames(typeof(SupportedFileExt)).Aggregate((x, y) => $"{x}, {y}");
    Console.WriteLine($"Supported types: {supportedTypes}");
    return;
}
Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
{
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    ConverterToXml.Core.ConverterToXml.Convert(parsedArgs, Console.Out, Console.Error);
});