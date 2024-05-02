namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(120)]
public class FlowChartConnectorType : ShapeType
{
    public FlowChartConnectorType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.round;
        
        Path = "m10800,qx,10800,10800,21600,21600,10800,10800,xe";
        
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163";
        
        TextboxRectangle = "3163,3163,18437,18437";
    }
}