namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(56)]
public class PentagonType : ShapeType
{
    public PentagonType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m10800,l,8259,4200,21600r13200,l21600,8259xe";
        
        ConnectorLocations = "10800,0;0,8259;4200,21600;10800,21600;17400,21600;21600,8259";
        
        ConnectorAngles = "270,180,90,90,90,0";
        
        TextboxRectangle = "4200,5077,17400,21600";
    }
}