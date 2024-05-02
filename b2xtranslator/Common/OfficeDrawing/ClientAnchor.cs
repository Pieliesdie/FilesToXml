using System;
using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF010)]
public class ClientAnchor : Record
{
    public byte[] Bytes;
    
    public ClientAnchor(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        Bytes = Reader.ReadBytes((int)BodySize);
    }
    
    //these are only valid for Powerpoint
    public int Top => BitConverter.ToInt16(Bytes, 0);
    public int Left => BitConverter.ToInt16(Bytes, 2);
    public int Right => BitConverter.ToInt16(Bytes, 4);
    public int Bottom => BitConverter.ToInt16(Bytes, 6);
}