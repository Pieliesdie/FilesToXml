namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(119)]
public class FlowChartManualOperationType : ShapeType
{
    public FlowChartManualOperationType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l21600,,17240,21600r-12880,xe";
        
        ConnectorLocations = "10800,0;2180,10800;10800,21600;19420,10800";
        
        TextboxRectangle = "4321,0,17204,21600";
    }
}