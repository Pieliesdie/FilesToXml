namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(132)]
internal class FlowChartMagneticDiskType : ShapeType
{
    public FlowChartMagneticDiskType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m10800,qx,3391l,18209qy10800,21600,21600,18209l21600,3391qy10800,xem,3391nfqy10800,6782,21600,3391e";
        ConnectorLocations = "10800,6782;10800,0;0,10800;10800,21600;21600,10800";
        ConnectorAngles = "270,270,180,90,0";
        TextboxRectangle = "0,6782,21600,18209";
    }
}