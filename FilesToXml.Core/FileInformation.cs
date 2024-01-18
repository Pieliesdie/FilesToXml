using System;
using System.IO;
using System.Text;

namespace FilesToXml.Core;

public struct FileInformation : IDisposable
{
    public readonly string Path;
    public readonly string? Label;
    public readonly Encoding Encoding;
    public readonly SupportedFileExt? Type;
    public readonly string Delimiter;
    public readonly char[] SearchingDelimiters;
    private Stream? stream;
    public Stream Stream => stream ??= File.OpenRead(Path);
    public FileInformation(string path,
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

    public void Dispose() => stream?.Dispose();
}

