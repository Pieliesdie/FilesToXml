namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(1)]
public class RectangleType : ShapeType
{
    public RectangleType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xe";
    }
}