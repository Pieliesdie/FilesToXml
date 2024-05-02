namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(117)]
public class FlowChartPreparationType : ShapeType
{
    public FlowChartPreparationType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m4353,l17214,r4386,10800l17214,21600r-12861,l,10800xe";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "4353,0,17214,21600";
    }
}