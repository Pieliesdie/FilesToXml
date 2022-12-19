using Microsoft.JSInterop;
using System;
using System.Xml.Linq;

namespace ConverterWasm;

public partial class Program
{
    // Entry point is invoked by the JavaScript runtime on boot.
    public static void Main()
    {     
        Console.WriteLine($"DotNet here!");
    }

    [JSInvokable] // The method is invoked from JavaScript.
    public static string GetName() => "DotNet";

    [JSInvokable] // The method is invoked from JavaScript.
    public static string Formatter(string xml) => XElement.Parse(xml).ToString();

    //[JSInvokable]
    //public static void Convert(string[] args) => ConverterConsole.Program.Main(args);

    //[JSInvokable]
    //public static void Convert(Options args) => ConverterToXml.ConverterToXml.Convert(args, Console.Out, Console.Error);
}