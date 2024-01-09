using System;
using System.IO;
using System.Text;

namespace ConverterToXml.Core;

public struct ParsedFile : IDisposable
{
    public string Path;
    public string? Label;
    public Encoding Encoding;
    public SupportedFileExt? Type;
    public string Delimiter;
    public char[] SearchingDelimiters;
    private Stream? stream;

    public Stream Stream { get => stream ??= File.OpenRead(Path); private set => stream = value; }

    public ParsedFile(string path,
                      string? label,
                      Encoding encoding,
                      SupportedFileExt? type,
                      string delimiter,
                      char[] searchingDelimiters)
    {
        Path = path;
        Label = label;
        Encoding = encoding;
        Type = type;
        Delimiter = delimiter;
        SearchingDelimiters = searchingDelimiters;
    }

    public void Dispose()
    {
        stream?.Dispose();
    }
}

