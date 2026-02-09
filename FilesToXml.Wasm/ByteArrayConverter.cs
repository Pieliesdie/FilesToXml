using System.Text.Json;
using System.Text.Json.Serialization;

namespace FilesToXml.Wasm;

public class ByteArrayConverter : JsonConverter<byte[]>
{
    public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var bytes = new List<byte>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;
                bytes.Add(reader.GetByte());
            }
            return bytes.ToArray();
        }
        // Fallback to base64 string
        return Convert.FromBase64String(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var b in value)
            writer.WriteNumberValue(b);
        writer.WriteEndArray();
    }
}