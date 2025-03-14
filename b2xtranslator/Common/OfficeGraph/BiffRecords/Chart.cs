﻿using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the position and size of the chart area and specifies the beginning
///     of a collection of records as defined by the Chart Sheet Substream ABNF. The collection of records specifies a
///     chart.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Chart)]
public class Chart : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Chart;
    /// <summary>
    ///     A FixedPoint as specified in [MS-OSHARED] section 2.2.1.6 that specifies
    ///     the width in points.
    ///     MUST be greater than or equal to 0.
    /// </summary>
    private FixedPointNumber dx;
    /// <summary>
    ///     A FixedPoint as specified in [MS-OSHARED] section 2.2.1.6 that specifies
    ///     the height in points.
    ///     MUST be greater than or equal to 0.
    /// </summary>
    private FixedPointNumber dy;
    /// <summary>
    ///     A FixedPoint as specified in [MS-OSHARED] section 2.2.1.6 that specifies
    ///     the horizontal position of the upper-left corner of the chart in points.
    ///     MUST be 0.
    /// </summary>
    private FixedPointNumber x;
    /// <summary>
    ///     A FixedPoint as specified in [MS-OSHARED] section 2.2.1.6 that specifies
    ///     the vertical position of the upper-left corner of the chart in points.
    ///     MUST be 0.
    /// </summary>
    private FixedPointNumber y;
    
    public Chart(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        x = new FixedPointNumber(reader);
        y = new FixedPointNumber(reader);
        dx = new FixedPointNumber(reader);
        dy = new FixedPointNumber(reader);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}