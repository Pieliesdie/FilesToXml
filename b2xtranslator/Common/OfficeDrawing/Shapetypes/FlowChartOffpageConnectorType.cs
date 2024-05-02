namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(177)]
internal class FlowChartOffpageConnectorType : ShapeType
{
    public FlowChartOffpageConnectorType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m,l21600,r,17255l10800,21600,,17255xe";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "0,0,21600,17255";
    }
}