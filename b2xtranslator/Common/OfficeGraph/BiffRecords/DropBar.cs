﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the attributes of the up bars or the down bars between multiple
///     series of a line chart group and specifies the beginning of a collection of records
///     as defined by the Chart Sheet Substream ABNF. The first of these collections in the
///     line chart group specifies the attributes of the up bars. The second specifies the
///     attributes of the down bars. If this record exists, then the chart group type
///     MUST be line and the field cSer in the record SeriesList MUST be greater than 1.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.DropBar)]
public class DropBar : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.DropBar;
    /// <summary>
    ///     A signed integer that specifies the width of the gap between the up bars or the down bars.
    ///     MUST be a value between 0 and 500.
    ///     The width of the gap in SPRCs can be calculated by the following formula:
    ///     Width of the gap in SPRCs = 1 + pcGap
    /// </summary>
    public short pcGap;
    
    public DropBar(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        pcGap = reader.ReadInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}