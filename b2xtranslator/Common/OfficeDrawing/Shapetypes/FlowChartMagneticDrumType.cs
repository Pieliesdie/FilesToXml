namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(133)]
internal class FlowChartMagneticDrumType : ShapeType
{
    public FlowChartMagneticDrumType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m21600,10800qy18019,21600l3581,21600qx,10800,3581,l18019,qx21600,10800xem18019,21600nfqx14438,10800,18019,e";
        ConnectorLocations = "10800,0;0,10800;10800,21600;14438,10800;21600,10800";
        ConnectorAngles = "270,180,90,0,0";
        TextboxRectangle = "3581,0,14438,21600";
    }
}