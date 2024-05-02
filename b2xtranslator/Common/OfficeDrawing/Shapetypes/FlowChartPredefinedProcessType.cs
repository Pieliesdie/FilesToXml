namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(112)]
public class FlowChartPredefinedProcessType : ShapeType
{
    public FlowChartPredefinedProcessType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xem2610,nfl2610,21600em18990,nfl18990,21600e";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "2610,0,18990,21600";
    }
}