using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bootsharp;
using FilesToXml.Core;

public static partial class Program
{
    public static void Main () { }
    
    [JSInvokable]
    public static string GetBackendName() => $".NET {Environment.Version}";
    
    [JSInvokable]
    public static string Beautify(string xml) => XDocument.Parse(xml).ToString();
    
    [JSInvokable]
    public static string? Convert(Options options)
    {
        return null;
    }
}
public record FileOption
{
    public required string Path { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Codepage { get; set; } = 65001;
    public string Delimiter { get; set; } = "auto";
}
public struct Options
{
    public required IEnumerable<FileOption> Input { get; set; }
    public string Output { get; set; }
    public int OutputCodepage { get; set; }
    public bool ForceSave { get; set; }
    public bool DisableFormat { get; set; }
    public string[] SearchingDelimiters { get; set; }
}