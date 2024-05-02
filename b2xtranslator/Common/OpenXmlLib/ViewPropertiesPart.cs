namespace b2xtranslator.OpenXmlLib;

public class ViewPropertiesPart : OpenXmlPart
{
    internal ViewPropertiesPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => PresentationMLContentTypes.ViewProps;
    public override string RelationshipType => OpenXmlNamespaces.viewProps;
    public override string TargetName => "viewProps";
    public override string TargetDirectory => "";
}