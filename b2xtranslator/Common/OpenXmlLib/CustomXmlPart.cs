namespace b2xtranslator.OpenXmlLib;

public class CustomXmlPart : OpenXmlPart
{
    public CustomXmlPart(OpenXmlPackage package, int partIndex)
        : base(package, partIndex) { }
    
    public override string ContentType => OpenXmlContentTypes.Xml;
    public override string RelationshipType => OpenXmlRelationshipTypes.CustomXml;
    public override string TargetName => "item" + PartIndex;
    public override string TargetDirectory => @"customXml";
}