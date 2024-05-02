namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class EndnotesPart : ContentPart
{
    public EndnotesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Endnotes;
    public override string RelationshipType => OpenXmlRelationshipTypes.Endnotes;
    public override string TargetName => "endnotes";
    public override string TargetDirectory => "";
}