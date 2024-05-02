using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies font information used on the chart and specifies
///     the beginning of a collection of Font records as defined by the Chart Sheet Substream ABNF.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.FrtFontList)]
public class FrtFontList : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.FrtFontList;
    /// <summary>
    ///     An unsigned integer that specifies the number of items in rgFontInfo.
    /// </summary>
    public ushort cFont;
    /// <summary>
    ///     An FrtHeaderOld. The frtHeaderOld.rt field MUST be 0x085A.
    /// </summary>
    private FrtHeaderOld frtHeaderOld;
    /// <summary>
    ///     An array of FontInfo structures that specify the font information.
    ///     The number of elements in this array MUST be equal to the value specified in cFont.
    /// </summary>
    public FontInfo[] rgFontInfo;
    /// <summary>
    ///     An unsigned integer that specifies the application version where new chart elements
    ///     were introduced that use the font information specified by rgFontInfo.
    ///     MUST be equal to iObjectInstance1 of the StartObject record that immediately
    ///     follows this record as defined by the Chart Sheet Substream ABNF and
    ///     MUST be a value from the following table:
    ///     Value       Meaning
    ///     0x09        This record pertains to new objects introduced in a specific
    ///     version of the application <53>. rgFontInfo specifies the font
    ///     information that is used by display units labels specified by YMult.
    ///     0x0A        This record pertains to new objects introduced in specific version of
    ///     the application <54>. rgFontInfo specifies the font information that
    ///     is used by extended data labels specified by DataLabExt.
    /// </summary>
    public byte verExcel;
    
    public FrtFontList(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        frtHeaderOld = new FrtHeaderOld(reader);
        verExcel = reader.ReadByte();
        
        // skip reserved byte
        reader.ReadByte();
        
        cFont = reader.ReadUInt16();
        
        if (cFont > 0)
        {
            rgFontInfo = new FontInfo[cFont];
            
            for (var i = 0; i < cFont; i++)
            {
                rgFontInfo[i] = new FontInfo(reader);
            }
        }
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}