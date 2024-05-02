using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the location of the OLE server window that is contained in the parent document window when
///     the chart data was saved.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.MainWindow)]
public class MainWindow : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.MainWindow;
    /// <summary>
    ///     A signed integer that specifies the height of the window in twips.<br />
    ///     MUST be greater than or equal to 0.
    /// </summary>
    public short wHeight;
    /// <summary>
    ///     A signed integer that specifies the location in twips of the left edge
    ///     of the window relative to the left edge of the primary monitor.
    /// </summary>
    public short wLeft;
    /// <summary>
    ///     A signed integer that specifies the location in twips of the top edge
    ///     of the window relative to the top edge of the primary monitor.
    /// </summary>
    public short wTop;
    /// <summary>
    ///     A signed integer that specifies the width of the window in twips.<br />
    ///     MUST be greater than or equal to 0.
    /// </summary>
    public short wWidth;
    
    public MainWindow(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        wLeft = reader.ReadInt16();
        wTop = reader.ReadInt16();
        wWidth = reader.ReadInt16();
        wHeight = reader.ReadInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}