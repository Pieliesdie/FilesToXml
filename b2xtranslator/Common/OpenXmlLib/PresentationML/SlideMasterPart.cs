namespace b2xtranslator.OpenXmlLib.PresentationML;

public class SlideMasterPart : ContentPart
{
    protected static int _slideLayoutCounter;
    
    public SlideMasterPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.SlideMaster;
    public override string RelationshipType => OpenXmlRelationshipTypes.SlideMaster;
    public override string TargetName => "slideMaster" + PartIndex;
    public override string TargetDirectory => "slideMasters";
    
    public SlideLayoutPart AddSlideLayoutPart()
    {
        var part = new SlideLayoutPart(this, ++_slideLayoutCounter);
        part.ReferencePart(this);
        return AddPart(part);
    }
}