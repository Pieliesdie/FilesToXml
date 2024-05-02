namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(73)]
internal class LightningBoltType : ShapeType
{
    public LightningBoltType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m8472,l,3890,7602,8382,5022,9705r7200,4192l10012,14915r11588,6685l14767,12877r1810,-870l11050,6797r1810,-717xe";
        
        ConnectorLocations = "8472,0;0,3890;5022,9705;10012,14915;21600,21600;16577,12007;12860,6080";
        ConnectorAngles = "270,270,180,180,90,0,0";
        TextboxRectangle = "8757,7437,13917,14277";
    }
}