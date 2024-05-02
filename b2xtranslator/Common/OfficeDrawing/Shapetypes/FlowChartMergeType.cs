namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(128)]
internal class FlowChartMergeType : ShapeType
{
    public FlowChartMergeType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m,l21600,,10800,21600xe";
        ConnectorLocations = "10800,0;5400,10800;10800,21600;16200,10800";
        TextboxRectangle = "5400,0,16200,10800";
    }
}