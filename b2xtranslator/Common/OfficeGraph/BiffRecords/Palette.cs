using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies a custom color palette.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Palette)]
public class Palette : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Palette;
    /// <summary>
    ///     A signed integer that specifies the number of colors in the rgColor array.
    ///     The value MUST be 56.
    /// </summary>
    public short ccv;
    /// <summary>
    ///     An array of LongRGB that specifies the colors of the color palette.
    ///     The number of items in the array MUST be equal to the value specified in the ccv field.
    /// </summary>
    public RGBColor[] rgColor;
    
    public Palette(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        ccv = reader.ReadInt16();
        rgColor = new RGBColor[ccv];
        for (var i = 0; i < ccv; i++)
        {
            rgColor[i] = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
        }
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}