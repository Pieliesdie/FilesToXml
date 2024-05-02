namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class FooterPart : ContentPart
{
    public FooterPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Footer;
    public override string RelationshipType => OpenXmlRelationshipTypes.Footer;
    public override string TargetName => "footer" + PartIndex;
    public override string TargetDirectory => "";
}