namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(113)]
public class FlowChartInternalStorageType : ShapeType
{
    public FlowChartInternalStorageType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xem4236,nfl4236,21600em,4236nfl21600,4236e";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "4236,4236,21600,21600";
    }
}