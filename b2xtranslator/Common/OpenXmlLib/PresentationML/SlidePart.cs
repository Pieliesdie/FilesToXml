namespace b2xtranslator.OpenXmlLib.PresentationML;

public class SlidePart : ContentPart
{
    public SlidePart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.Slide;
    public override string RelationshipType => OpenXmlRelationshipTypes.Slide;
    public override string TargetName => "slide" + PartIndex;
    public override string TargetDirectory => "slides";
}