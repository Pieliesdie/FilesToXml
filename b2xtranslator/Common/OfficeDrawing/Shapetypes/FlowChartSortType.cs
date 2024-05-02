namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(126)]
internal class FlowChartSortType : ShapeType
{
    public FlowChartSortType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m10800,l,10800,10800,21600,21600,10800xem,10800nfl21600,10800e";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "5400,5400,16200,16200";
    }
}