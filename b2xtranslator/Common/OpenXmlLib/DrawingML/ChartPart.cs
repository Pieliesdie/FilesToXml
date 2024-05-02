namespace b2xtranslator.OpenXmlLib.DrawingML;

public class ChartPart : OpenXmlPart
{
    public ChartPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => DrawingMLContentTypes.Chart;
    public override string RelationshipType => OpenXmlRelationshipTypes.Chart;
    public override string TargetName => "chart" + PartIndex;
    public override string TargetDirectory => "../charts";
}