using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the number of axis groups on the chart.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.AxesUsed)]
public class AxesUsed : OfficeGraphBiffRecord
{
    public enum AxisGroupsPresent : ushort
    {
        /// <summary>
        ///     A single primary axis group is present
        /// </summary>
        Primary = 0x1,
        /// <summary>
        ///     Both a primary axis group and a secondary axis group are present
        /// </summary>
        PrimaryAndSecondary = 0x2
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.AxesUsed;
    /// <summary>
    ///     An unsigned integer that specifies the number of axis groups on the chart.
    /// </summary>
    public AxisGroupsPresent cAxes = AxisGroupsPresent.Primary;
    
    public AxesUsed(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        cAxes = (AxisGroupsPresent)reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}