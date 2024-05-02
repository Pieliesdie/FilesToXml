using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies properties of a chart as defined by the Chart Sheet Substream ABNF.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.ShtProps)]
public class ShtProps : OfficeGraphBiffRecord
{
    public enum EmptyCellPlotMode
    {
        PlotNothing,
        PlotAsZero,
        PlotAsInterpolated
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.ShtProps;
    /// <summary>
    ///     A bit that specifies whether the default plot area dimension is used.<br />
    ///     MUST be a value from the following table:
    /// </summary>
    public bool fAlwaysAutoPlotArea;
    /// <summary>
    ///     If fAlwaysAutoPlotArea is true then this field MUST be true.
    ///     If fAlwaysAutoPlotArea is false then this field MUST be ignored.
    /// </summary>
    public bool fManPlotArea;
    /// <summary>
    ///     A bit that specifies whether series are automatically allocated for the chart.
    /// </summary>
    public bool fManSerAlloc;
    /// <summary>
    ///     Specifies how the empty cells are plotted.
    /// </summary>
    public EmptyCellPlotMode mdBlank;
    
    public ShtProps(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        var flags = reader.ReadUInt16();
        fManSerAlloc = Utils.BitmaskToBool(flags, 0x1);
        // 0x2 and 0x4 are reserved
        fManPlotArea = Utils.BitmaskToBool(flags, 0x8);
        fAlwaysAutoPlotArea = Utils.BitmaskToBool(flags, 0x10);
        mdBlank = (EmptyCellPlotMode)reader.ReadByte();
        reader.ReadByte(); // skip the last byte
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}