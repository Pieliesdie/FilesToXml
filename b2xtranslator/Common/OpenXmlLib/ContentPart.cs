namespace b2xtranslator.OpenXmlLib;

public abstract class ContentPart : OpenXmlPart
{
    public ContentPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public ContentPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public ImagePart AddImagePart(ImagePart.ImageType type)
    {
        return AddPart(new ImagePart(type, this, Package.GetNextImageId()));
    }
    
    public EmbeddedObjectPart AddEmbeddedObjectPart(EmbeddedObjectPart.ObjectType type)
    {
        return AddPart(new EmbeddedObjectPart(type, this, Package.GetNextOleId()));
    }
    
    public VmlPart AddVmlPart()
    {
        return AddPart(new VmlPart(this, Package.GetNextVmlId()));
    }
}