﻿using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the contents of an extended data label.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.DataLabExtContents)]
public class DataLabExtContents : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.DataLabExtContents;
    /// <summary>
    ///     A bit that specifies whether the bubble size is displayed in the data label.
    ///     MUST equal 0 if the chart group type of the corresponding chart group,
    ///     series, or data point is not a bubble chart group.
    /// </summary>
    public bool fBubSizes;
    /// <summary>
    ///     fCatName (1 bit): A bit that specifies whether the category (3) name, or the horizontal
    ///     value on bubble or scatter chart groups, is displayed in the extended data label.
    ///     MUST be a value from the following table:
    ///     Value   Meaning
    ///     0       Neither of the data values are displayed in the extended data label.
    ///     1       If bubble or scatter chart group, the horizontal value is displayed
    ///     in the extended data label. Otherwise, the category (3) name is
    ///     displayed in the extended data label.
    /// </summary>
    public bool fCatName;
    /// <summary>
    ///     A bit that specifies whether the value of the corresponding data point, represented as a
    ///     percentage of the sum of the values of the series the data label is associated with, is
    ///     displayed in the extended data label.
    ///     MUST equal 0 if the chart group type of the corresponding chart group, series,
    ///     or data point is not a bar of pie, doughnut, pie, or pie of pie chart group.
    /// </summary>
    public bool fPercent;
    /// <summary>
    ///     An FrtHeader. The FrtHeader.rt field MUST be 0x086B.
    /// </summary>
    public FrtHeader frtHeader;
    /// <summary>
    ///     A bit that specifies whether the name of the series is displayed in the extended data label.
    /// </summary>
    public bool fSerName;
    /// <summary>
    ///     A bit that specifies whether the data value, or the vertical value on bubble or scatter
    ///     chart groups, is displayed in the extended data label. MUST be a value from the following table:
    ///     Value   Meaning
    ///     0       Neither of the data values are displayed in the data label.
    ///     1       If bubble or scatter chart group, the vertical value is displayed
    ///     in the extended data label. Otherwise, the data value is displayed
    ///     in the extended data label.
    /// </summary>
    public bool fValue;
    /// <summary>
    ///     A case-sensitive XLUnicodeStringMin2 that specifies the string that is
    ///     inserted between every data value to form the extended data label. For example, if
    ///     fCatName and fValue are set to 1, the labels will look like “Category Name
    ///     <value of
    ///         rgchSep>
    ///         Data Value”. The length of the string is contained in the
    ///         cch field of the XLUnicodeStringMin2 structure.
    /// </summary>
    public XLUnicodeStringMin2 rgchSep;
    
    public DataLabExtContents(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        frtHeader = new FrtHeader(reader);
        
        var flags = reader.ReadUInt16();
        fSerName = Utils.BitmaskToBool(flags, 0x0001);
        fCatName = Utils.BitmaskToBool(flags, 0x0002);
        fValue = Utils.BitmaskToBool(flags, 0x0004);
        fPercent = Utils.BitmaskToBool(flags, 0x0008);
        fBubSizes = Utils.BitmaskToBool(flags, 0x0010);
        
        rgchSep = new XLUnicodeStringMin2(reader);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}