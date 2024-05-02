namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(123)]
internal class FlowChartSummingJunctionType : ShapeType
{
    public FlowChartSummingJunctionType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m10800,qx,10800,10800,21600,21600,10800,10800,xem3163,3163nfl18437,18437em3163,18437nfl18437,3163e";
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163";
        TextboxRectangle = "3163,3163,18437,18437";
    }
}