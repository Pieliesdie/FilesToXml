namespace b2xtranslator.OpenXmlLib;

public class CorePropertiesPart : OpenXmlPart
{
    internal CorePropertiesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => OpenXmlContentTypes.CoreProperties;
    public override string RelationshipType => OpenXmlRelationshipTypes.CoreProperties;
    public override string TargetName => "core";
    public override string TargetDirectory => "docProps";
}