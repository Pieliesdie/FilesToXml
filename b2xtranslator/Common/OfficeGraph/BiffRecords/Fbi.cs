using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the font information at the time the scalable font is added to the chart. <47>
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Fbi)]
public class Fbi : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Fbi;
    /// <summary>
    ///     An unsigned integer that specifies the font width, in twips, when the font was first applied.
    ///     MUST be greater than or equal to 0 and less than or equal to 0x7FFF.
    /// </summary>
    public ushort dmixBasis;
    /// <summary>
    ///     An unsigned integer that specifies the font height, in twips, when the font was first applied.
    ///     MUST be greater than or equal to 0 and less than or equal to 0x7FFF.
    /// </summary>
    public ushort dmiyBasis;
    public ushort ifnt;
    /// <summary>
    ///     A Boolean that specifies the scale to use. The value MUST be one of the following values:
    ///     Value       Meaning
    ///     0x0000      Scale by chart area
    ///     0x0001      Scale by plot area
    /// </summary>
    public bool scab;
    /// <summary>
    ///     An unsigned integer that specifies the default font height in twips.
    ///     MUST be greater than or equal to 20 and less than or equal to 8180.
    /// </summary>
    public ushort twpHeightBasis;
    // TODO: implement FontIndex???
    
    public Fbi(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        dmixBasis = reader.ReadUInt16();
        dmiyBasis = reader.ReadUInt16();
        twpHeightBasis = reader.ReadUInt16();
        scab = reader.ReadUInt16() == 0x0001;
        ifnt = reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}