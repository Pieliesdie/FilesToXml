using System;
using System.Text.Json;
using System.Xml.Linq;

using FilesToXml.Core;

namespace FilesToXml.Wasm;

public partial class Program
{
    // Entry point is invoked by the JavaScript runtime on boot.
    public static void Main()
    {
        Console.WriteLine($"DotNet here!");
    }

    public static void Convert(
        string[] input,
        string output,
        char[]? searchingDelimiters = null,
        string[]? delimiters = null,
        int[]? inputEncoding = null,
        string[]? labels = null,
        int outputEncoding = 65001,
        bool disableFormat = false,
        bool forceSave = false)
    {
        searchingDelimiters ??= new[] { ';', '|', '\t', ',' };
        delimiters ??= new[] { "auto" };
        inputEncoding ??= new[] { 65001 };
        labels ??= Array.Empty<string>();
        var options = new WasmOptions(input)
        {
            Delimiters = delimiters,
            InputEncoding = inputEncoding,
            Labels = labels,
            Output = output,
            SearchingDelimiters = searchingDelimiters,
            DisableFormat = disableFormat,
            ForceSave = forceSave,
            OutputEncoding = outputEncoding
        };

        ConverterToXml.Convert(options, Console.Out, Console.Error);
    }
}
