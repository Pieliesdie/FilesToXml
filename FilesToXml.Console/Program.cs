using CommandLine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using FilesToXml.Console;
using FilesToXml.Core;

var isSupportCommand = args.FirstOrDefault(x => x == "--support") is not null;
if (isSupportCommand)
{
    var supportedTypes = Enum.GetNames(typeof(SupportedFileExt)).Aggregate((x, y) => $"{x}, {y}");
    Console.WriteLine($"Supported types: {supportedTypes}");
    return;
}
var isPrintCodepages = args.FirstOrDefault(x => x == "--codepages") is not null;
if (isPrintCodepages)
{
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    Console.WriteLine($"codepages: ");
    foreach (var encodingInfo in Encoding.GetEncodings())
    {
        Console.WriteLine($"{encodingInfo.CodePage} - {encodingInfo.Name} - {encodingInfo.DisplayName}");
    }
    return;
}
Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
{
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    ConverterToXml.Convert(parsedArgs, Console.OpenStandardOutput(), Console.OpenStandardError());
});

#if DEBUG
Console.ReadKey();
#endif