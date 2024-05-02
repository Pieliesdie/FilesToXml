namespace b2xtranslator.OpenXmlLib;

public class EmbeddedObjectPart : OpenXmlPart
{
    public enum ObjectType
    {
        Excel,
        Word,
        Powerpoint,
        Other
    }
    
    private readonly ObjectType _format;
    
    //public override string TargetDirectory { get { return "embeddings"; } }
    
    public EmbeddedObjectPart(ObjectType format, OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex)
    {
        _format = format;
    }
    
    public override string ContentType
    {
        get
        {
            switch (_format)
            {
                case ObjectType.Excel:
                    return OpenXmlContentTypes.MSExcel;
                case ObjectType.Word:
                    return OpenXmlContentTypes.MSWord;
                case ObjectType.Powerpoint:
                    return OpenXmlContentTypes.MSPowerpoint;
                case ObjectType.Other:
                    return OpenXmlContentTypes.OleObject;
                default:
                    return OpenXmlContentTypes.OleObject;
            }
        }
    }
    
    internal override bool HasDefaultContentType => true;
    public override string RelationshipType => OpenXmlRelationshipTypes.OleObject;
    public override string TargetName => "oleObject" + PartIndex;
    public override string TargetDirectory { get; set; } = "embeddings";
    
    public override string TargetExt
    {
        get
        {
            switch (_format)
            {
                case ObjectType.Excel:
                    return ".xls";
                case ObjectType.Word:
                    return ".doc";
                case ObjectType.Powerpoint:
                    return ".ppt";
                case ObjectType.Other:
                    return ".bin";
                default:
                    return ".bin";
            }
        }
    }
}