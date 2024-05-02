namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(121)]
internal class FlowChartPunchedCardType : ShapeType
{
    public FlowChartPunchedCardType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m4321,l21600,r,21600l,21600,,4338xe";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "0,4321,21600,21600";
    }
}