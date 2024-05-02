using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies a custom color palette for a chart sheet.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.ClrtClient)]
public class ClrtClient : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.ClrtClient;
    /// <summary>
    ///     A signed integer that specifies the number of colors in the rgColor array.
    ///     MUST be 0x0003.
    /// </summary>
    public short ccv;
    /// <summary>
    ///     An array of LongRGB. The array specifies the colors of the color palette.
    ///     MUST contain the following values:
    ///     Index       Element             Value
    ///     0           Foreground color    This value MUST be equal to the system window text color of the system palette
    ///     1           Background color    This value MUST be equal to the system window color of the system palette
    ///     2           Neutral color       This value MUST be black
    /// </summary>
    private readonly RGBColor[] rgColor;
    
    public ClrtClient(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        ccv = reader.ReadInt16();
        
        if (ccv > 0)
        {
            rgColor = new RGBColor[ccv];
            
            for (var i = 0; i < ccv; i++)
            {
                rgColor[i] = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
            }
        }
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}