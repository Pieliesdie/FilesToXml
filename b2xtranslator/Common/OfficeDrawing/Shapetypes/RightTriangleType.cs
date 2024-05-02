namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(6)]
public class RightTriangleType : ShapeType
{
    public RightTriangleType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,xe";
        
        ConnectorLocations = "0,0;0,10800;0,21600;10800,21600;21600,21600;10800,10800";
        
        TextboxRectangle = "1800,12600,12600,19800";
    }
}