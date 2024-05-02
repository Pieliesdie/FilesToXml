using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class PathSegment
{
    public enum SegmentType
    {
        msopathLineTo,
        msopathCurveTo,
        msopathMoveTo,
        msopathClose,
        msopathEnd,
        msopathEscape,
        msopathClientEscape,
        msopathInvalid
    }
    
    public PathSegment(ushort segment)
    {
        Type = (SegmentType)Utils.BitmaskToInt(segment, 0xE000);
        
        if (Type == SegmentType.msopathEscape)
        {
            EscapeCode = Utils.BitmaskToInt(segment, 0x1F00);
            VertexCount = Utils.BitmaskToInt(segment, 0x00FF);
        }
        else
        {
            Count = Utils.BitmaskToInt(segment, 0x1FFF);
        }
    }
    
    public SegmentType Type { get; set; }
    public int Count { get; set; }
    public int EscapeCode { get; set; }
    public int VertexCount { get; set; }
}