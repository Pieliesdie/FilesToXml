namespace b2xtranslator.OpenXmlLib;

public class ThemePart : OpenXmlPart
{
    public ThemePart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => OpenXmlContentTypes.Theme;
    public override string RelationshipType => OpenXmlRelationshipTypes.Theme;
    public override string TargetName => "theme" + PartIndex;
    public override string TargetDirectory => "theme";
}