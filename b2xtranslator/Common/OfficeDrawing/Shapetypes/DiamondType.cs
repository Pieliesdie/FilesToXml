namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(4)]
public class DiamondType : ShapeType
{
    public DiamondType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m10800,l,10800,10800,21600,21600,10800xe";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "5400,5400,16200,16200";
    }
}