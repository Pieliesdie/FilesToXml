using System.IO;
using System.Text;

namespace FilesToXml.WPF.Helpers;

public class LineStream : MemoryStream
{
    public delegate void WriteLineEventHandler(string line);
    
    public delegate void WriteLinesEventHandler(string[] lines);
    
    private readonly Encoding encoding;
    private readonly StringBuilder partialLineBuffer = new();
    
    public LineStream(Encoding encoding)
    {
        this.encoding = encoding.RemovePreamble();
    }
    
    public event WriteLineEventHandler? OnWriteLine;
    public event WriteLinesEventHandler? OnWriteLines;
    
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        base.Write(buffer);
        if (buffer.Length > 0)
        {
            ProcessBuffer(buffer);
        }
    }
    
    private void ProcessBuffer(ReadOnlySpan<byte> buffer)
    {
        if (OnWriteLine is null && OnWriteLines is null)
        {
            return;
        }
        
        // Convert the buffer to a string
        var content = encoding.GetString(buffer);
        
        // Combine the partialLineBuffer with the new content
        content = partialLineBuffer + content;
        
        // Split the content into lines
        var lines = content.Split(Environment.NewLine);
        
        // The last element in the array might be a partial line
        // Store it in partialLineBuffer for processing in the next Write call
        partialLineBuffer.Clear();
        if (content[^1] != '\n')
        {
            partialLineBuffer.Append(lines[^1]);
            lines = lines[..^1];
        }
        else if (content[^1] == '\n' && lines.LastOrDefault() == string.Empty)
        {
            lines = lines[..^1];
        }
        
        // Process complete lines
        for (var i = 0; i < lines.Length - 1; i++)
        {
            OnWriteLine?.Invoke(lines[i]);
        }
        
        // Invoke the OnWriteLines event with all lines (including the potential partial line)
        if (lines.Length > 0)
        {
            OnWriteLines?.Invoke(lines);
        }
    }
    
    #region Disposing
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // flush buffer if something in it
            if (partialLineBuffer.Length > 0)
            {
                var nonFlushedString = partialLineBuffer.ToString();
                OnWriteLine?.Invoke(nonFlushedString);
                OnWriteLines?.Invoke([nonFlushedString]);
            }
            
            Cleanup();
        }
        
        base.Dispose(disposing);
    }
    
    private void Cleanup()
    {
        OnWriteLine = null;
        OnWriteLines = null;
    }
    
    #endregion
}