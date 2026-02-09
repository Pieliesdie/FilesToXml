using System.Text.Json.Serialization;
using FilesToXml.Core.Defaults;

namespace FilesToXml.Wasm;

public class File : DefaultFile
{
    [JsonConverter(typeof(ByteArrayConverter))]
    public byte[] Data { get; set; } = [];
    
    protected override bool TryOpenStreamInternal(string path, TextWriter err, out Stream? stream)
    {
        stream = new MemoryStream(Data);
        return true;
    }
}