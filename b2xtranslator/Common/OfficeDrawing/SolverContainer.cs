using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF005)]
public class SolverContainer : RegularContainer
{
    public SolverContainer(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        foreach (var item in Children)
        {
            switch (item.TypeCode) { }
        }
    }
}

[OfficeRecord(0xF012)]
public class FConnectorRule : Record
{
    public uint cptiA;
    public uint cptiB;
    public uint ruid;
    public uint spidA;
    public uint spidB;
    public uint spidC;
    
    public FConnectorRule(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        ruid = Reader.ReadUInt32();
        spidA = Reader.ReadUInt32();
        spidB = Reader.ReadUInt32();
        spidC = Reader.ReadUInt32();
        cptiA = Reader.ReadUInt32();
        cptiB = Reader.ReadUInt32();
    }
}

[OfficeRecord(0xF014)]
public class FArcRule : Record
{
    public uint ruid;
    public uint spid;
    
    public FArcRule(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        ruid = Reader.ReadUInt32();
        spid = Reader.ReadUInt32();
    }
}

[OfficeRecord(0xF017)]
public class FCalloutRule : Record
{
    public uint ruid;
    public uint spid;
    
    public FCalloutRule(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        ruid = Reader.ReadUInt32();
        spid = Reader.ReadUInt32();
    }
}