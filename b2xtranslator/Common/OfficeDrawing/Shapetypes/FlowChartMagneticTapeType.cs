namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(131)]
internal class FlowChartMagneticTapeType : ShapeType
{
    public FlowChartMagneticTapeType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "ar,,21600,21600,18685,18165,10677,21597l20990,21597r,-3432xe";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "3163,3163,18437,18437";
    }
}