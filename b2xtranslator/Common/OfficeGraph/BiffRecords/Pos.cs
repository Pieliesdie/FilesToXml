﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the size and position for a legend, an attached label,
///     or the plot area, as specified by the primary axis group. <br />
///     This record MUST be ignored for the plot area when the fManPlotArea field of ShtProps is set to 1.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Pos)]
public class Pos : OfficeGraphBiffRecord
{
    public enum PositionMode
    {
        MDFX,
        MDABS,
        MDPARENT,
        MDKTH,
        MDCHART
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.Pos;
    /// <summary>
    ///     A PositionMode that specifies the positioning mode for the lower-right corner of a legend,
    ///     an attached label, or the plot area.
    /// </summary>
    public PositionMode mdBotRt;
    /// <summary>
    ///     A PositionMode that specifies the positioning mode for the upper-left corner of a legend,
    ///     an attached label, or the plot area.
    /// </summary>
    public PositionMode mdTopLt;
    /// <summary>
    ///     A signed integer that specifies a position. <br />
    ///     The meaning is specified in the Valid Combinations of mdTopLt and mdBotRt by Type table.
    /// </summary>
    public short x1;
    /// <summary>
    ///     A signed integer that specifies a position. <br />
    ///     The meaning is specified in the Valid Combinations of mdTopLt and mdBotRt by Type table.
    /// </summary>
    public short x2;
    /// <summary>
    ///     A signed integer that specifies a position. <br />
    ///     The meaning is specified in the Valid Combinations of mdTopLt and mdBotRt by Type table.
    /// </summary>
    public short y1;
    /// <summary>
    ///     A signed integer that specifies a position. <br />
    ///     The meaning is specified in the Valid Combinations of mdTopLt and mdBotRt by Type table.
    /// </summary>
    public short y2;
    
    public Pos(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        mdTopLt = (PositionMode)reader.ReadInt16();
        mdBotRt = (PositionMode)reader.ReadInt16();
        x1 = reader.ReadInt16();
        reader.ReadBytes(2); // skip 2 bytes
        y1 = reader.ReadInt16();
        reader.ReadBytes(2); // skip 2 bytes
        x2 = reader.ReadInt16();
        reader.ReadBytes(2); // skip 2 bytes
        y2 = reader.ReadInt16();
        reader.ReadBytes(2); // skip 2 bytes
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}