namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(125)]
internal class FlowChartCollateType : ShapeType
{
    public FlowChartCollateType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m21600,21600l,21600,21600,,,xe";
        ConnectorLocations = "10800,0;10800,10800;10800,21600";
        TextboxRectangle = "5400,5400,16200,16200";
    }
}