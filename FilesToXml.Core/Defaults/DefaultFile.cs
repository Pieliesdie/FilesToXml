using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultFile : DefaultFileOptions, IFile
{
    private Stream? openedStream;
    public bool TryGetStream(TextWriter err, [NotNullWhen(true)] out Stream? stream)
    {
        var opened = TryOpenStream(Path, err, out stream);
        openedStream = stream;
        return opened;
    }
    public void Dispose() => openedStream?.Dispose();
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
}