namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class StyleDefinitionsPart : OpenXmlPart
{
    public StyleDefinitionsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Styles;
    public override string RelationshipType => OpenXmlRelationshipTypes.Styles;
    public override string TargetName => "styles";
    public override string TargetDirectory => "";
}