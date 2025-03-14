﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies that the chart group is an area chart group and specifies the chart group attributes.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Area)]
public class Area : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Area;
    /// <summary>
    ///     A bit that specifies whether the data points in the chart group are displayed as a
    ///     percentage of the sum of all data points in the chart group that share the same category (3).
    ///     MUST be 0 if fStacked is 0.
    /// </summary>
    public bool f100;
    /// <summary>
    ///     A bit that specifies whether one or more data points in the chart group has shadows.
    /// </summary>
    public bool fHasShadow;
    /// <summary>
    ///     A bit that specifies whether the data points in the chart group that share the same category (3) are stacked one on
    ///     top of the next.
    /// </summary>
    public bool fStacked;
    
    public Area(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        var flags = reader.ReadUInt16();
        fStacked = Utils.BitmaskToBool(flags, 0x1);
        f100 = Utils.BitmaskToBool(flags, 0x2);
        fHasShadow = Utils.BitmaskToBool(flags, 0x4);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}