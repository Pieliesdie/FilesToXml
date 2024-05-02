namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(127)]
internal class FlowChartExtractType : ShapeType
{
    public FlowChartExtractType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m10800,l21600,21600,,21600xe";
        ConnectorLocations = "10800,0;5400,10800;10800,21600;16200,10800";
        TextboxRectangle = "5400,10800,16200,21600";
    }
}