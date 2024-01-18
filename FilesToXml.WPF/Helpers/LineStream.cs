using System.IO;
using System.Text;

namespace FilesToXml.WPF.Helpers;

public class LineStream : MemoryStream
{
    private readonly Encoding encoding;
    private readonly StringBuilder partialLineBuffer = new();
    public delegate void WriteLineEventHandler(string line);
    public event WriteLineEventHandler? OnWriteLine;
    public delegate void WriteLinesEventHandler(string[] lines);
    public event WriteLinesEventHandler? OnWriteLines;
    public LineStream(Encoding encoding)
    {
        this.encoding = encoding;
    }
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        base.Write(buffer);
        ProcessBuffer(buffer);
    }
    private void ProcessBuffer(ReadOnlySpan<byte> buffer)
    {
        // Convert the buffer to a string
        string content = encoding.GetString(buffer);

        // Combine the partialLineBuffer with the new content
        content = partialLineBuffer + content;

        // Split the content into lines
        string[] lines = content.Split(Environment.NewLine);

        // The last element in the array might be a partial line
        // Store it in partialLineBuffer for processing in the next Write call
        partialLineBuffer.Clear();
        if (content[^1] != '\n')
        {
            partialLineBuffer.Append(lines[^1]);
            lines = lines[..^1];
        }
        
        // Process complete lines
        for (int i = 0; i < lines.Length - 1; i++)
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
