namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class FontTablePart : OpenXmlPart
{
    public FontTablePart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.FontTable;
    public override string RelationshipType => OpenXmlRelationshipTypes.FontTable;
    public override string TargetName => "fontTable";
    public override string TargetDirectory => "";
}