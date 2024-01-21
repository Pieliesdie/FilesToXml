using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Bootsharp;
using FilesToXml.Core;

public static partial class Program
{
    public static void Main () { }
    
    [JSInvokable]
    public static string GetBackendName() => $"hello from .NET {Environment.Version}";
    
    [JSInvokable]
    public static string Beautify(string xml) => XDocument.Parse(xml).ToString();
    
    [JSInvokable]
    public static string? Convert()
    {
        return null;
    }
}

public struct Test
{
    public readonly string Path;
    public readonly string? Label;
    public readonly string EncodingName;
    public Filetype? Type;
    public readonly string Delimiter;
    public readonly char[] SearchingDelimiters;

    public Test(string path,
        string? label,
        string encoding,
        Filetype? type,
        string delimiter,
        char[] searchingDelimiters)
    {
        Path = path;
        Label = label;
        EncodingName = encoding;
        Type = type;
        Delimiter = delimiter;
        SearchingDelimiters = searchingDelimiters;
    }
}