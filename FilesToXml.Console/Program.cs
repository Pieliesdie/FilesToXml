using System;
using System.Linq;
using System.Text;
using CommandLine;
using FilesToXml.Console;
using FilesToXml.Core;
using FilesToXml.Core.Extensions;

var isSupportCommand = args.Any(x => x == "--support");
if (isSupportCommand)
{
    Console.WriteLine("Supported types:");
    Enum.GetNames<Filetype>().ForEach(Console.WriteLine);
    return;
}

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
var isPrintCodepages = args.Any(x => x == "--codepages");
if (isPrintCodepages)
{
    Console.WriteLine("Codepages: ");
    Encoding.GetEncodings().ForEach(encod => Console.WriteLine($"{encod.CodePage} - {encod.Name} - {encod.DisplayName}"));
    return;
}

Parser.Default.ParseArguments<Options>(args).WithParsed(parsedArgs =>
{
    ConverterToXml.Convert(parsedArgs, Console.OpenStandardOutput(), Console.OpenStandardError());
});