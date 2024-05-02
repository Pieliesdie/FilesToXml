using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the text in a text box or a form control.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.TxO)]
public class TxO : OfficeGraphBiffRecord
{
    public enum HorizontalAlignment : ushort
    {
        Left = 1,
        Centered = 2,
        Right = 3,
        Justify = 4,
        JustifyDistributed = 7
    }
    
    public enum VerticalAlignment : ushort
    {
        Top = 1,
        Middle = 2,
        Bottom = 3,
        Justify = 4,
        JustifyDistributed = 7
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.TxO;
    /// <summary>
    ///     An unsigned integer that specifies the number of bytes of formatting run information in the
    ///     TxORuns structure contained in the Continue records following this record.<br />
    ///     If cchText is 0, this value MUST be 0.<br />
    ///     Otherwise the value MUST be greater than or equal to 16 and MUST be a multiple of 8.
    /// </summary>
    public ushort cbRuns;
    /// <summary>
    ///     An unsigned integer that specifies the number of characters in the text string
    ///     contained in the Continue records immediately following this record. <br />
    ///     MUST be less than or equal to 255.
    /// </summary>
    public ushort cchText;
    /// <summary>
    ///     Specifies the horizontal alignment.
    /// </summary>
    public HorizontalAlignment hAlignment;
    /// <summary>
    ///     A FontIndex that specifies the font when cchText is 0.<br />
    /// </summary>
    public ushort ifntEmpty;
    /// <summary>
    ///     Specifies the orientation of the text within the object boundary.
    /// </summary>
    public TextRotation rot;
    /// <summary>
    ///     Specifies the vertical alignment.
    /// </summary>
    public VerticalAlignment vAlignment;
    
    public TxO(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        var flags = reader.ReadUInt16();
        hAlignment = (HorizontalAlignment)Utils.BitmaskToInt(flags, 0xE);
        vAlignment = (VerticalAlignment)Utils.BitmaskToInt(flags, 0x70);
        rot = (TextRotation)reader.ReadUInt16();
        reader.ReadBytes(6); // reserved
        cchText = reader.ReadUInt16();
        cbRuns = reader.ReadUInt16();
        ifntEmpty = reader.ReadUInt16();
        reader.ReadBytes(2); // reserved
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}