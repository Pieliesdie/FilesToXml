using FilesToXml.Core.Defaults;

namespace FilesToXml.Wasm;

public class File : DefaultFile
{
    public string Data { get; set; } = string.Empty;
    
    protected override bool TryOpenStream(string path, TextWriter err, out Stream? stream)
    {
        stream = new MemoryStream(Convert.FromBase64String(Data));
        return true;
    }
}