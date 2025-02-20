﻿using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.PlotGrowth)]
public class PlotGrowth : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.PlotGrowth;
    /// <summary>
    ///     A FixedPoint that specifies the horizontal growth (in points) of the plot area for font scaling.
    /// </summary>
    public FixedPointNumber dxPlotGrowth;
    /// <summary>
    ///     A FixedPoint that specifies the vertical growth (in points) of the plot area for font scaling.
    /// </summary>
    public FixedPointNumber dyPlotGrowth;
    
    public PlotGrowth(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        dxPlotGrowth = new FixedPointNumber(reader);
        dyPlotGrowth = new FixedPointNumber(reader);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}