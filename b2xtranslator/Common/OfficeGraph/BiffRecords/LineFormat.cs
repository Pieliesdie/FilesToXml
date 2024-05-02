﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the appearance of a line.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.LineFormat)]
public class LineFormat : OfficeGraphBiffRecord
{
    public enum LineStyle
    {
        Solid,
        Dash,
        Dot,
        DashDot,
        DashDotDot,
        None,
        DarkGrayPattern,
        MediumGrayPattern,
        LightGrayPattern
    }
    
    public enum LineWeight
    {
        Hairline = -1,
        Narrow = 0,
        Medium = 1,
        Wide = 2
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.LineFormat;
    /// <summary>
    ///     A bit that specifies whether the line has default formatting.<br />
    ///     If the value is false, the line has formatting as specified by lns, we, and icv.<br />
    ///     If the value is true, lns, we, icv, and rgb MUST be ignored and default values are used as specified in the
    ///     following table:<br />
    ///     lns = Solid<br />
    ///     we = Narrow<br />
    ///     icv = 0x004D<br />
    ///     rgb = Match the default color used for icv<br />
    /// </summary>
    public bool fAuto;
    /// <summary>
    ///     A bit that specifies whether icv equals 0x004D.<br />
    ///     If the value is true, icv MUST equal 0x004D. <br />
    ///     If the value is false, icv MUST NOT equal 0x004D.
    /// </summary>
    public bool fAutoCo;
    /// <summary>
    ///     A bit that specifies whether the axis line is displayed.
    /// </summary>
    public bool fAxisOn;
    /// <summary>
    ///     An unsigned integer that specifies the color of the line.
    ///     The value SHOULD be an IcvChart value. <br />
    ///     The value MUST be an IcvChart value, 0x0040, or 0x0041. <br />
    ///     The color MUST match the color specified by rgb.<br />
    /// </summary>
    public ushort icv;
    /// <summary>
    ///     Specifies the style of the line.
    /// </summary>
    public LineStyle lns;
    /// <summary>
    ///     Specifies the color of the line.
    /// </summary>
    public RGBColor rgb;
    /// <summary>
    ///     Specifies the thickness of the line.
    /// </summary>
    public LineWeight we;
    
    public LineFormat(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        rgb = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
        lns = (LineStyle)reader.ReadInt16();
        we = (LineWeight)reader.ReadInt16();
        var flags = reader.ReadUInt16();
        fAuto = Utils.BitmaskToBool(flags, 0x1);
        // 0x2 is reserved
        fAxisOn = Utils.BitmaskToBool(flags, 0x4);
        fAutoCo = Utils.BitmaskToBool(flags, 0x8);
        icv = reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}