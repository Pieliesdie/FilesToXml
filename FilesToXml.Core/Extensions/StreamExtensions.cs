using System.Text;

namespace FilesToXml.Core.Extensions;

public static class StreamExtensions
{
    public static IEnumerable<string> ReadAllLines(this StreamReader reader)
    {
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line is null)
            {
                continue;
            }
            
            yield return line;
        }
    }
    
    public static IEnumerable<string> ReadAllLinesWithNewLine(this Stream stream, Encoding encoding)
    {
        using var sr = new StreamReader(stream, encoding);
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (line is null)
            {
                continue;
            }
            
            yield return line;
            yield return Environment.NewLine;
        }
    }
    
    public static void ResetStream(params StreamWriter[] streams)
    {
        foreach (var stream in streams)
        {
            if (stream.BaseStream.CanSeek)
            {
                stream.BaseStream.Position = 0;
            }
        }
    }
    
    public static void WriteTo(this Stream source, Stream destination, int bufferSize = 1024, bool leaveOpen = false)
    {
        // Create a buffer to store the read data
        var buffer = new byte[bufferSize];
        
        // Read from the source stream and write to the destination stream
        int bytesRead;
        while ((bytesRead = source.Read(buffer, 0, bufferSize)) > 0)
        {
            destination.Write(buffer, 0, bytesRead);
        }
        
        if (!leaveOpen)
        {
            source.Dispose();
        }
    }
}