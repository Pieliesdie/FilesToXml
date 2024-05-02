namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(116)]
public class FlowChartTerminatorType : ShapeType
{
    public FlowChartTerminatorType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.round;
        
        Path = "m3475,qx,10800,3475,21600l18125,21600qx21600,10800,18125,xe";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "1018,3163,20582,18437";
    }
}