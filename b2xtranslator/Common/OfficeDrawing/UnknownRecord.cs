using System.IO;

namespace b2xtranslator.OfficeDrawing;

public class UnknownRecord : Record
{
    public UnknownRecord(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        if (Reader.BaseStream.Length - Reader.BaseStream.Position >= size)
        {
            Reader.ReadBytes((int)size);
        }
        else
        {
            Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position));
        }
    }
}