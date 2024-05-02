using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the size and position of the chart window within the OLE server window that
///     is contained in the parent document window. When this record follows a MainWindow record,
///     to define the position of the data sheet window, the Window1_10 record MUST be used.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Window1)]
public class Window1 : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Window1;
    /// <summary>
    ///     An unsigned integer that specifies the width of the chart window within
    ///     the OLE server window, in twips. MUST be greater than or equal to 0x0001.
    /// </summary>
    public ushort dxWn;
    /// <summary>
    ///     An unsigned integer that specifies the height of the chart window within
    ///     the OLE server window, in twips. MUST be greater than or equal to 0x0001.
    /// </summary>
    public ushort dyWn;
    /// <summary>
    ///     A signed integer that specifies the X location of the upper-left corner of the chart
    ///     window within the OLE server window, in twips. SHOULD <73> be greater than or equal to zero.
    /// </summary>
    public short xWn;
    /// <summary>
    ///     A signed integer that specifies the Y location of the upper-left corner of the chart
    ///     window within the OLE server window, in twips. SHOULD <74> be greater than or equal to zero.
    /// </summary>
    public short yWn;
    
    public Window1(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        xWn = reader.ReadInt16();
        yWn = reader.ReadInt16();
        dxWn = reader.ReadUInt16();
        dyWn = reader.ReadUInt16();
        
        // skipped reserved and unused bytes
        reader.ReadBytes(10);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}