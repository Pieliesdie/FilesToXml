using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies properties of a Future Record Type (FRT) as defined
///     by the Chart Sheet Substream ABNF. The collection of records specifies a
///     feature saved as an FRT such that an application not supporting
///     the feature can preserve it.
///     This record MUST have an associated StartObject record. StartObject and
///     EndObject pairs can be nested. Up to 100 levels of blocks can be nested.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.EndObject)]
public class EndObject : OfficeGraphBiffRecord
{
    public enum ObjectKind : ushort
    {
        YMult = 0x0010,
        FrtFontList = 0x0011,
        DataLabExt = 0x0012
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.EndObject;
    /// <summary>
    ///     An FrtHeaderOld. The frtHeaderOld.rt field MUST be 0x0855.
    /// </summary>
    public FrtHeaderOld frtHeaderOld;
    /// <summary>
    ///     An unsigned integer that specifies the type of object that is encompassed by the block.
    ///     MUST equal the iObjectKind field of the associated StartObject record.
    /// </summary>
    public ObjectKind iObjectKind;
    
    public EndObject(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        frtHeaderOld = new FrtHeaderOld(reader);
        iObjectKind = (ObjectKind)reader.ReadUInt16();
        
        reader.ReadBytes(6);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}