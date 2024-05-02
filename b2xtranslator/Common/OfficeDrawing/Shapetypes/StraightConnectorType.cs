namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(32)]
public class StraightConnectorType : ShapeType
{
    public StraightConnectorType()
    {
        Path = "m,l21600,21600e";
        ConnectorLocations = "0,0;21600,21600";
    }
}