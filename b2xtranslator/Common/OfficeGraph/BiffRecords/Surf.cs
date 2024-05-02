using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies that the chart group is a surface chart group and specifies the chart group attributes.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Surf)]
public class Surf : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Surf;
    /// <summary>
    ///     A bit that specifies whether 3-D Phong shading is displayed.
    /// </summary>
    public bool f3DPhongShade;
    /// <summary>
    ///     A bit that specifies whether the surface chart group is wireframe or has a fill.<br />
    ///     true = Surface chart group has a fill.<br />
    ///     false = Surface chart group is wireframe.
    /// </summary>
    public bool fFillSurface;
    
    public Surf(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        var flags = reader.ReadUInt16();
        fFillSurface = Utils.BitmaskToBool(flags, 0x1);
        f3DPhongShade = Utils.BitmaskToBool(flags, 0x2);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}