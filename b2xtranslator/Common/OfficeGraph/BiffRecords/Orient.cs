using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies how the series data of a chart is arranged.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Orient)]
public class Orient : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Orient;
    /// <summary>
    ///     An unsigned integer that specifies a zero-based column index into the data sheet.<br />
    ///     The referenced column is used to calculate axis values along the horizontal value
    ///     axis in a scatter chart group. MUST be 0x0000 for other chart group types.<br />
    ///     MUST equal to rowSeriesX and MUST be ignored if fSeriesInRows is 0x01.
    /// </summary>
    public ushort colSeriesX;
    /// <summary>
    ///     A Boolean that specifies whether series are arranged by rows or columns from the
    ///     data specified in a data sheet window. <br />
    ///     MUST be a value from the following table:<br />
    ///     false = Series are arranged by columns.<br />
    ///     true = Series are arranged by rows.
    /// </summary>
    public bool fSeriesInRows;
    /// <summary>
    ///     An unsigned integer that specifies a zero-based row index into the data sheet. <br />
    ///     The referenced row is used to calculate axis values along the horizontal value axis in a
    ///     scatter chart group. MUST be 0x0000 for other chart group types. <br />
    ///     MUST equal to colSeriesX and MUST be ignored if fSeriesInRows is 0x00.
    /// </summary>
    public ushort rowSeriesX;
    
    public Orient(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        fSeriesInRows = Utils.ByteToBool(reader.ReadByte());
        rowSeriesX = reader.ReadUInt16();
        colSeriesX = reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}