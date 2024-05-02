namespace b2xtranslator.OpenXmlLib.PresentationML;

public class SlideLayoutPart : ContentPart
{
    public SlideLayoutPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.SlideLayout;
    public override string RelationshipType => OpenXmlRelationshipTypes.SlideLayout;
    public override string TargetName => "slideLayout" + PartIndex;
    public override string TargetDirectory => "..\\slideLayouts";
}