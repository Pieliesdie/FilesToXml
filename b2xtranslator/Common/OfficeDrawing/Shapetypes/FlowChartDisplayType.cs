namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(134)]
internal class FlowChartDisplayType : ShapeType
{
    public FlowChartDisplayType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m17955,v862,282,1877,1410,2477,3045c21035,5357,21372,7895,21597,10827v-225,2763,-562,5300,-1165,7613c19832,20132,18817,21260,17955,21597r-14388,l,10827,3567,xe";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "3567,0,17955,21600";
    }
}