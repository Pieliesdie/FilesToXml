using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultFileOptions : IFileOptions
{
    private Stream? openedStream;
    public required string Path { get; init; }
    public int CodePage { get; init; } = 65001;
    public string? Label { get; init; }
    public string? Delimiter { get; set; }
    public char[]? SearchingDelimiters { get; set; }
    public bool TryGetData(TextWriter err, [NotNullWhen(true)] out Stream? stream)
    {
        var opened = TryOpenStream(Path, err, out stream);
        openedStream = stream;
        return opened;
    }
    protected virtual bool TryOpenStream(string path, TextWriter err, out Stream? stream)
    {
        stream = null;
        try
        {
            stream = File.OpenRead(path);
            return true;
        }
        catch (Exception ex)
        {
            err.WriteLine($"'{path}': {ex.Message}");
            return false;
        }
    }
    public void Dispose()
    {
        openedStream?.Dispose();
    }
}