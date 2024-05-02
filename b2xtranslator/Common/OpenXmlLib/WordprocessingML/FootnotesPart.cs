namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class FootnotesPart : ContentPart
{
    public FootnotesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Footnotes;
    public override string RelationshipType => OpenXmlRelationshipTypes.Footnotes;
    public override string TargetName => "footnotes";
    public override string TargetDirectory => "";
}