namespace b2xtranslator.OpenXmlLib;

public class VbaDataPart : ContentPart
{
    internal VbaDataPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => MicrosoftWordContentTypes.VbaData;
    public override string RelationshipType => MicrosoftWordRelationshipTypes.VbaData;
    public override string TargetName => "vbaData";
    public override string TargetExt => ".xml";
    public override string TargetDirectory => "";
}