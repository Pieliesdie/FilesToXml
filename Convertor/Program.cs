using CommandLine;
using ConverterToXml;
using System;
using System.Linq;
using System.Text;

namespace ConverterConsole;

public partial class Program
{
    public static void Main(string[] args)
    {
        var isSupportCommand = args.FirstOrDefault(x => x == "--support") is not null;
        if (isSupportCommand)
        {
            var supportedTypes = Enum.GetNames(typeof(SupportedFileExt)).Aggregate((x, y) => $"{x}, {y}");
            Console.WriteLine($"Supported types: {supportedTypes}");
            return;
        }
        Parser.Default.ParseArguments<Options>(args).WithParsed(args =>
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ConverterToXml.ConverterToXml.Convert(args, Console.Out, Console.Error);
        });
    }
    
}
