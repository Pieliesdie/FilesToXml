namespace b2xtranslator.OpenXmlLib;

public class AppPropertiesPart : OpenXmlPart
{
    internal AppPropertiesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => OpenXmlContentTypes.ExtendedProperties;
    public override string RelationshipType => OpenXmlRelationshipTypes.ExtendedProperties;
    public override string TargetName => "app";
    public override string TargetDirectory => "docProps";
}