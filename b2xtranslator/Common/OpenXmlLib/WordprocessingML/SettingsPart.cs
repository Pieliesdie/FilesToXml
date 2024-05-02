namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class SettingsPart : OpenXmlPart
{
    internal SettingsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Settings;
    public override string RelationshipType => OpenXmlRelationshipTypes.Settings;
    public override string TargetName => "settings";
    public override string TargetDirectory => "";
}