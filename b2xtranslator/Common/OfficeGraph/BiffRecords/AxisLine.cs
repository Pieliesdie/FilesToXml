﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies which part of the axis is specified by the LineFormat record that follows.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.AxisLine)]
public class AxisLine : OfficeGraphBiffRecord
{
    public enum AxisPart : ushort
    {
        /// <summary>
        ///     The axis line itself
        /// </summary>
        AxisLine = 0x0,
        /// <summary>
        ///     The major gridlines along the axis
        /// </summary>
        MajorGridlines = 0x1,
        /// <summary>
        ///     The minor gridlines along the axis
        /// </summary>
        MinorGridlines = 0x2,
        /// <summary>
        ///     The walls or floor of a 3-D chart
        /// </summary>
        WallOrFloor3D = 0x3
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.AxisLine;
    /// <summary>
    ///     An unsigned integer that specifies which part of the axis is defined
    ///     by the LineFormat record that follows.
    ///     MUST be unique among all other id field values in AxisLine records in the current axis.
    ///     MUST be greater than the id field values in preceding AxisLine records in the current axis.
    /// </summary>
    public AxisPart axisId;
    
    public AxisLine(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        axisId = (AxisPart)reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}