namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class SharedStringPart : OpenXmlPart
{
    public SharedStringPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => SpreadsheetMLContentTypes.SharedStrings;
    public override string RelationshipType => OpenXmlRelationshipTypes.SharedStrings;
    public override string TargetName => "sharedStrings";
    public override string TargetDirectory => "";
}