﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the color, size, and shape of the associated data markers that
///     appear on line, radar, and scatter chart groups. The associated data markers are specified
///     by the preceding DataFormat record. If this record is not present in the sequence of records
///     that conforms to the SS rule then all the fields will have default values otherwise all
///     the fields MUST contain a value.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.MarkerFormat)]
public class MarkerFormat : OfficeGraphBiffRecord
{
    public enum MarkerType
    {
        NoMarker,
        SquareMarkers,
        DiamondShapedMarkers,
        TriangularMarkers,
        SquareMarkersWithX,
        SquareMarkersWithAsterisk,
        ShortBarMarkers,
        LongBarMarkers,
        CircularMarkers,
        SquareMarkersWithPlus
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.MarkerFormat;
    /// <summary>
    ///     A bit that specifies whether the data marker is automatically generated.
    /// </summary>
    public bool fAuto;
    /// <summary>
    ///     A bit that specifies whether to show the data marker border.
    /// </summary>
    public bool fNotShowBrd;
    /// <summary>
    ///     A bit that specifies whether to show the data marker interior.
    /// </summary>
    public bool fNotShowInt;
    /// <summary>
    ///     An unsigned integer that specifies the interior color of the data marker.<br />
    ///     The value SHOULD <59> be an IcvChart value. <br />
    ///     The value MUST be an IcvChart value, between 0x0000 and 0x0007 (inclusively), or between 0x0040 and 0x0041
    ///     (inclusively).<br />
    ///     The color MUST match the color specified by rgbBack. <br />
    ///     The default value of this field is automatically set to match the color specified by rgbBack.
    /// </summary>
    public ushort icvBack;
    /// <summary>
    ///     An unsigned integer that specifies the border color of the data marker.<br />
    ///     The value SHOULD <58> be an IcvChart value. <br />
    ///     The value MUST be an IcvChart value, between 0x0000 and 0x0007 (inclusively), or between 0x0040 and 0x0041
    ///     (inclusively).  <br />
    ///     The color MUST match the color specified by rgbFore.  <br />
    ///     The default value of this field is automatically set to match the color specified by rgbFore.
    /// </summary>
    public ushort icvFore;
    /// <summary>
    ///     An unsigned integer that specifies the type of data marker.
    /// </summary>
    public MarkerType imk;
    /// <summary>
    ///     An unsigned integer that specifies the size in twips of the data marker. <br />
    ///     MUST be greater than or equal to 40 and less than or equal to 1440. <br />
    ///     The default value for this field is 100.
    /// </summary>
    public uint miSize;
    /// <summary>
    ///     Specifies the interior color of the data marker.<br />
    ///     The color MUST match the color specified by icvBack. <br />
    ///     The default value of this field is the same as the default value
    ///     for rgbFore only when the default imk is 0x0001, 0x0002, 0x0003, or 0x0008 otherwise the default value is 0xFFFFFF.
    /// </summary>
    public RGBColor rgbBack;
    /// <summary>
    ///     Specifies the border color of the data marker. <br />
    ///     The color MUST match the color specified by icvFore. <br />
    ///     The default value of this field is automatically selected from the next available color in the Chart color table
    /// </summary>
    public RGBColor rgbFore;
    
    public MarkerFormat(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        rgbFore = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
        rgbBack = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
        imk = (MarkerType)reader.ReadUInt16();
        var flags = reader.ReadUInt16();
        fAuto = Utils.BitmaskToBool(flags, 0x1);
        //0x2 - 0x8 are reserved
        fNotShowInt = Utils.BitmaskToBool(flags, 0x10);
        fNotShowBrd = Utils.BitmaskToBool(flags, 0x20);
        icvFore = reader.ReadUInt16();
        icvBack = reader.ReadUInt16();
        miSize = reader.ReadUInt32();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}