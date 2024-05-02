﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.Legend)]
public class Legend : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Legend;
    /// <summary>
    ///     An unsigned integer that specifies the width, in SPRC, of the bounding rectangle of the legend.<br />
    ///     MUST be ignored and the x2 field from the following Pos record MUST be used instead.
    /// </summary>
    public uint dx;
    /// <summary>
    ///     An unsigned integer that specifies the height, in SPRC, of the bounding rectangle of the legend.<br />
    ///     MUST be ignored and the y2 field from the following Pos record MUST be used instead.
    /// </summary>
    public uint dy;
    /// <summary>
    ///     A bit that specifies whether the legend is automatically positioned.
    ///     If this field is true, then fAutoPosX MUST be true and fAutoPosY MUST be true.
    /// </summary>
    public bool fAutoPosition;
    /// <summary>
    ///     A bit that specifies whether the x-positioning of the legend is automatic.
    /// </summary>
    public bool fAutoPosX;
    /// <summary>
    ///     A bit that specifies whether the y-positioning of the legend is automatic.
    /// </summary>
    public bool fAutoPosY;
    /// <summary>
    ///     A bit that specifies the layout of the legend entries. <br />
    ///     True: The legend contains a single column of legend entries.<br />
    ///     False: The legend contains multiple columns of legend entries or the size of the legend has been manually changed
    ///     from the default size.
    /// </summary>
    public bool fVert;
    /// <summary>
    ///     A bit that specifies whether the legend is shown in a data table.
    /// </summary>
    public bool fWasDataTable;
    /// <summary>
    ///     An unsigned integer that specifies the space between legend entries.<br />
    ///     MUST be 0x01 which represents 40 twips between legend entries.
    /// </summary>
    public byte wSpace;
    /// <summary>
    ///     An unsigned integer that specifies the x-position, in SPRC,
    ///     of the upper-left corner of the bounding rectangle of the legend. <br />
    ///     MUST be ignored and the x1 field from the following Pos record MUST be used instead.
    /// </summary>
    public uint x;
    /// <summary>
    ///     An unsigned integer that specifies the y-position, in SPRC,
    ///     of the upper-left corner of the bounding rectangle of the legend. <br />
    ///     MUST be ignored and the y1 field from the following Pos record MUST be used instead.
    /// </summary>
    public uint y;
    
    public Legend(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        x = reader.ReadUInt32();
        y = reader.ReadUInt32();
        dx = reader.ReadUInt32();
        dy = reader.ReadUInt32();
        reader.ReadByte(); // undefined
        wSpace = reader.ReadByte();
        
        var flags = reader.ReadUInt16();
        fAutoPosition = Utils.BitmaskToBool(flags, 0x1);
        //0x2 is reserved
        fAutoPosX = Utils.BitmaskToBool(flags, 0x4);
        fAutoPosY = Utils.BitmaskToBool(flags, 0x8);
        fVert = Utils.BitmaskToBool(flags, 0x10);
        fWasDataTable = Utils.BitmaskToBool(flags, 0x20);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}