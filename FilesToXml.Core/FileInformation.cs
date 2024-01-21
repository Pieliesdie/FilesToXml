using System;
using System.IO;
using System.Text;

namespace FilesToXml.Core;

public struct FileInformation : IDisposable
{
    public required string Name { get; init; }
    public required Stream Stream { get; init; }
    public required Filetype Type { get; init; }
    public required Encoding Encoding { get; init; }
    public string? Label { get; set; }
    public string? Delimiter { get; set; }
    public char[]? SearchingDelimiters { get; set; }
    public string? Path { get; set; }
    
    public FileInformation() {}
    public void Dispose()
    {
        Stream?.Dispose();
    }
}