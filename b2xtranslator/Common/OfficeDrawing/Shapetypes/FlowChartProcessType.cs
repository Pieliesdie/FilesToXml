namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(109)]
public class FlowChartProcessType : ShapeType
{
    public FlowChartProcessType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xe";
        ConnectorLocations = "Rectangle";
    }
}