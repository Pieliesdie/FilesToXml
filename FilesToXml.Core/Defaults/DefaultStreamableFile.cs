using System.Diagnostics.CodeAnalysis;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultStreamableFile : DefaultFileOptions, IFile
{
    public required Stream Stream { get; init; }
    
    [SetsRequiredMembers]
    public DefaultStreamableFile(Stream stream, string fileName)
    {
        Stream = stream;
        Path = $"{fileName}";
    }
    
    public bool TryGetStream(TextWriter err, [NotNullWhen(true)] out Stream? stream)
    {
        stream = null;
        try
        {
            if (!Stream.CanRead)
            {
                err.WriteLine($"Can't open stream for '{Path}'");
                return false;
            }
            
            Stream.Position = 0;
            stream = Stream;
            return true;
        }
        catch (Exception ex)
        {
            err.WriteLine(ex);
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
            Stream.Dispose();
        }
    }
}