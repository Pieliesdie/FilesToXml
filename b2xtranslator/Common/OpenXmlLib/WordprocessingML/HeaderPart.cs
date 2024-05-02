namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class HeaderPart : ContentPart
{
    public HeaderPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Header;
    public override string RelationshipType => OpenXmlRelationshipTypes.Header;
    public override string TargetName => "header" + PartIndex;
    public override string TargetDirectory => "";
}