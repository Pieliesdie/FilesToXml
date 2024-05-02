using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class SectionDescriptor : ByteStructure
{
    private const int SED_LENGTH = 12;
    public int fcMpr;
    /// <summary>
    ///     A signed integer that specifies the position in the WordDocument Stream where a Sepx structure is located.
    /// </summary>
    public int fcSepx;
    public short fn;
    public short fnMpr;
    
    public SectionDescriptor(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        fn = _reader.ReadInt16();
        fcSepx = _reader.ReadInt32();
        fnMpr = _reader.ReadInt16();
        fcMpr = _reader.ReadInt32();
    }
}