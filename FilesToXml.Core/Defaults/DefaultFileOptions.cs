using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesToXml.Core.Defaults;

public class DefaultFileOptions : IFileOptions
{
    public required string Path { get; init; }
    public required int InputEncoding { get; init; }
    public string? Label { get; init; }
    public string? Delimiter { get; set; }
    public IEnumerable<char>? SearchingDelimiters { get; set; }
    public bool TryGetData(TextWriter err, [NotNullWhen(true)] out Stream? stream)
    {
        return TryOpenStream(Path, err, out stream);
    }
    private static bool TryOpenStream(string path, TextWriter errWriter, [NotNullWhen(true)] out Stream? stream)
    {
        stream = null;
        try
        {
            stream = File.OpenRead(path);
            return true;
        }
        catch (Exception ex)
        {
            errWriter.WriteLine($"'{path}': {ex.Message}");
            return false;
        }
    }
}