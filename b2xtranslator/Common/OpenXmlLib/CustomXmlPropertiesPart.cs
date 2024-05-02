namespace b2xtranslator.OpenXmlLib;

public class CustomXmlPropertiesPart : OpenXmlPart
{
    public CustomXmlPropertiesPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => OpenXmlContentTypes.CustomXmlProperties;
    public override string RelationshipType => OpenXmlRelationshipTypes.CustomXmlProperties;
    public override string TargetName => "itemProps" + PartIndex;
    public override string TargetDirectory => "customXml";
}