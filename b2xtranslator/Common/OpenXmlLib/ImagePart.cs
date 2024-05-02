namespace b2xtranslator.OpenXmlLib;

public class ImagePart : OpenXmlPart
{
    public enum ImageType
    {
        Bmp,
        Emf,
        Gif,
        Icon,
        Jpeg,
        //Pcx,
        Png,
        Tiff,
        Wmf
    }
    
    protected ImageType _type;
    //public override string TargetDirectory { get { return "media"; } }
    
    internal ImagePart(ImageType type, OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex)
    {
        _type = type;
    }
    
    public override string ContentType
    {
        get
        {
            switch (_type)
            {
                case ImageType.Bmp:
                    return "image/bmp";
                case ImageType.Emf:
                    return "image/x-emf";
                case ImageType.Gif:
                    return "image/gif";
                case ImageType.Icon:
                    return "image/x-icon";
                case ImageType.Jpeg:
                    return "image/jpeg";
                //case ImagePartType.Pcx:
                //    return "image/pcx";
                case ImageType.Png:
                    return "image/png";
                case ImageType.Tiff:
                    return "image/tiff";
                case ImageType.Wmf:
                    return "image/x-wmf";
                default:
                    return "image/png";
            }
        }
    }
    
    public override string RelationshipType => OpenXmlRelationshipTypes.Image;
    public override string TargetName => "image" + PartIndex;
    public override string TargetDirectory { get; set; } = "media";
    
    public override string TargetExt
    {
        get
        {
            switch (_type)
            {
                case ImageType.Bmp:
                    return ".bmp";
                case ImageType.Emf:
                    return ".emf";
                case ImageType.Gif:
                    return ".gif";
                case ImageType.Icon:
                    return ".ico";
                case ImageType.Jpeg:
                    return ".jpg";
                //case ImagePartType.Pcx:
                //    return ".pcx";
                case ImageType.Png:
                    return ".png";
                case ImageType.Tiff:
                    return ".tif";
                case ImageType.Wmf:
                    return ".wmf";
                default:
                    return ".png";
            }
        }
    }
}