using System;
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
    public static string Beautify(FileInformation fileInformation)
    {
        return null;
    }
    
}