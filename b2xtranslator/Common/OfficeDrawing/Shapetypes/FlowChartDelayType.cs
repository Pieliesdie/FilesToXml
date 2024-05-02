namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(135)]
internal class FlowChartDelayType : ShapeType
{
    public FlowChartDelayType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m10800,qx21600,10800,10800,21600l,21600,,xe";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "0,3163,18437,18437";
    }
}