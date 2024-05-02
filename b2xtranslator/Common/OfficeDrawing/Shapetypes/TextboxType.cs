namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(202)]
public class TextboxType : ShapeType
{
    public TextboxType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xe";
    }
}