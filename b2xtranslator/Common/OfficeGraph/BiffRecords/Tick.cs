using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the attributes of the axis labels, major tick marks, and minor tick marks associated with an
///     axis.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Tick)]
public class Tick : OfficeGraphBiffRecord
{
    public enum MarkLabelLocation
    {
        Complex,
        Low,
        High,
        NextToAxis
    }
    
    public enum MarkLocation
    {
        None,
        Inside,
        Outside,
        Crossing
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.Tick;
    /// <summary>
    ///     A bit that specifies if the foreground text color of the axis labels is determined automatically.
    /// </summary>
    public bool fAutoCo;
    /// <summary>
    ///     A bit that specifies if the background color of the axis label is determined automatically.
    /// </summary>
    public bool fAutoMode;
    /// <summary>
    ///     A bit that specifies whether the text rotation of the axis label is determined automatically.
    /// </summary>
    public bool fAutoRot;
    /// <summary>
    ///     Specifies the color of the text in the color palette.
    /// </summary>
    public ushort icv;
    /// <summary>
    ///     specifies the reading order of the axis labels.
    /// </summary>
    public ReadingOrder iReadingOrder;
    /// <summary>
    ///     Specifies the color of the text for the axis labels.
    /// </summary>
    public RGBColor rgb;
    /// <summary>
    ///     Specifies text rotation of the axis labels. <br />
    ///     If Custom use value from trot.
    /// </summary>
    public TextRotation rot;
    /// <summary>
    ///     Specifies the location of major tick marks.
    /// </summary>
    public MarkLocation tktMajor;
    /// <summary>
    ///     Specifies the location of minor tick marks.
    /// </summary>
    public MarkLocation tktMinor;
    /// <summary>
    ///     Specifies the location of axis labels.
    /// </summary>
    public MarkLabelLocation tlt;
    /// <summary>
    ///     MUST be a value from the following table:<br />
    ///     0 to 90 = Text rotated 0 to 90 degrees counterclockwise<br />
    ///     91 to 180 = Text rotated 1 to 90 degrees clockwise (angle is trot – 90)<br />
    ///     255 = Text top-to-bottom with letters upright
    /// </summary>
    public ushort trot;
    /// <summary>
    ///     Specifies the display mode of the background of the text of the axis labels.
    /// </summary>
    public BackgroundMode wBkgMode;
    
    public Tick(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        tktMajor = (MarkLocation)reader.ReadByte();
        tktMinor = (MarkLocation)reader.ReadByte();
        tlt = (MarkLabelLocation)reader.ReadByte();
        wBkgMode = (BackgroundMode)reader.ReadByte();
        rgb = new RGBColor(reader.ReadInt32(), RGBColor.ByteOrder.RedFirst);
        reader.ReadBytes(16); // rerserved
        var flags = reader.ReadUInt16();
        fAutoCo = Utils.BitmaskToBool(flags, 0x1);
        fAutoMode = Utils.BitmaskToBool(flags, 0x2);
        rot = (TextRotation)Utils.BitmaskToInt(flags, 0x1C);
        fAutoRot = Utils.BitmaskToBool(flags, 0x10);
        iReadingOrder = (ReadingOrder)Utils.BitmaskToInt(flags, 0xC000);
        icv = reader.ReadUInt16();
        trot = reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}