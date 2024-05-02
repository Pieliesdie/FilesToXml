namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(3)]
public class OvalType : ShapeType
{
    public OvalType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.round;
    }
}