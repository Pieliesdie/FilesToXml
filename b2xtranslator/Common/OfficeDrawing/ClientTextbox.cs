using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF00D)]
public class ClientTextbox : Record
{
    public byte[] Bytes;
    
    public ClientTextbox(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        Bytes = Reader.ReadBytes((int)BodySize);
    }
}