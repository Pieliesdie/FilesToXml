using System.Diagnostics.CodeAnalysis;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultFile : DefaultFileOptions, IFile, IFileOptions, IStreambleData
{
    private Stream? openedStream;
    
    public bool TryGetStream(TextWriter err, [NotNullWhen(true)] out Stream? stream)
    {
        var opened = TryOpenStreamInternal(Path, err, out stream);
        openedStream = stream;
        return opened;
    }
    
    protected virtual bool TryOpenStreamInternal(string path, TextWriter err, out Stream? stream)
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            openedStream?.Dispose();
        }
    }
}