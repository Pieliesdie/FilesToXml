using System;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.JSInterop;

namespace FilesToXml.Wasm;

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

    [JSInvokable] // The method is invoked from JavaScript.
    public static string DefaultOptions()
    {
        return JsonSerializer.Serialize(new WasmOptions());
    }
    //[JSInvokable]
    //public static void Convert(string[] args) => ConverterConsole.Program.Main(args);

    [JSInvokable]
    public static void Convert(
        string[] input,
        string output,
        char[] searchingDelimiters = null,
        string[] delimiters = null,
        int[] inputEncoding = null,
        string[] labels = null,
        int outputEncoding = 65001,
        bool disableFormat = false,
        bool forceSave = false)
    {
        searchingDelimiters ??= new[] { ';', '|', '\t', ',' };
        delimiters ??= new[] { "auto" };
        inputEncoding ??= new[] { 65001 };
        labels ??= new string[0]; 
        var options = new WasmOptions
        {
            Delimiters = delimiters,
            InputEncoding = inputEncoding,
            Labels = labels,
            Output = output,
            SearchingDelimiters = searchingDelimiters,
            DisableFormat = disableFormat,
            ForceSave = forceSave,
            Input = input,
            OutputEncoding = outputEncoding
        };

        Core.ConverterToXml.Convert(options, Console.Out, Console.Error);
    }
}
