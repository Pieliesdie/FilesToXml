using System.ComponentModel;

namespace FilesToXml.Winform.Helpers;

public class StringWriterExt : StringWriter
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public delegate void FlushedEventHandler(object sender, EventArgs args);
    public event FlushedEventHandler? Flushed;
    public bool AutoFlush { get; set; } = true;

    public StringWriterExt()
        : base() { }

    public StringWriterExt(bool autoFlush)
        : base() { this.AutoFlush = autoFlush; }

    protected void OnFlush()
    {
        Flushed?.Invoke(this, EventArgs.Empty);
        this.GetStringBuilder().Clear();
    }

    public override void Flush()
    {
        base.Flush();
        OnFlush();
    }

    public override void Write(char value)
    {
        base.Write(value);
        if (AutoFlush) Flush();
    }

    public override void Write(string? value)
    {
        base.Write(value);
        if (AutoFlush) Flush();
    }
    public override void WriteLine(string? value)
    {
        base.WriteLine(value);
        if (AutoFlush) Flush();
    }

    public override void Write(char[] buffer, int index, int count)
    {
        base.Write(buffer, index, count);
        if (AutoFlush) Flush();
    }
}
