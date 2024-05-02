namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class ExternalLinkPart : OpenXmlPart
{
    private readonly int RefNumber;
    
    public ExternalLinkPart(OpenXmlPartContainer parent, int RefNumber)
        : base(parent, RefNumber)
    {
        this.RefNumber = RefNumber;
    }
    
    public override string ContentType => SpreadsheetMLContentTypes.ExternalLink;
    public override string RelationshipType => OpenXmlRelationshipTypes.ExternalLink;
    public override string TargetName => "externalLink" + RefNumber;
    public override string TargetDirectory => "externalLinks";
}