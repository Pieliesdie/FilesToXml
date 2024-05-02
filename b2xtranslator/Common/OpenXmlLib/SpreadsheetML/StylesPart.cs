namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class StylesPart : OpenXmlPart
{
    public StylesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => SpreadsheetMLContentTypes.Styles;
    public override string RelationshipType => OpenXmlRelationshipTypes.Styles;
    public override string TargetName => "styles";
    public override string TargetDirectory => "";
}