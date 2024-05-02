namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class NumberingDefinitionsPart : OpenXmlPart
{
    public NumberingDefinitionsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Numbering;
    public override string RelationshipType => OpenXmlRelationshipTypes.Numbering;
    public override string TargetName => "numbering";
    public override string TargetDirectory => "";
}