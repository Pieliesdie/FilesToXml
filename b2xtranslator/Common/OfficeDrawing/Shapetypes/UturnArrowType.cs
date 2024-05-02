namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(101)]
internal class UturnArrowType : ShapeType
{
    public UturnArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m15662,14285l21600,8310r-2970,qy9250,,,8485l,21600r6110,l6110,8310qy8907,5842l9725,5842qx12520,8310l9725,8310xe";
        
        ConnectorLocations = "9250,0;3055,21600;9725,8310;15662,14285;21600,8310";
        ConnectorAngles = "270,90,90,90,0";
        
        TextboxRectangle = "0,8310,6110,21600";
    }
}