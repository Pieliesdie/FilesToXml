namespace b2xtranslator.OpenXmlLib.DrawingML;

public class DrawingsPart : OpenXmlPart
{
    private static int _chartPartCount;
    
    public DrawingsPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => OpenXmlContentTypes.Drawing;
    public override string RelationshipType => OpenXmlRelationshipTypes.Drawing;
    public override string TargetName => "drawing" + PartIndex;
    public override string TargetDirectory => "../drawings";
    
    public ChartPart AddChartPart()
    {
        return AddPart(new ChartPart(this, ++_chartPartCount));
    }
}