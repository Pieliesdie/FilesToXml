namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class CommentsPart : ContentPart
{
    internal CommentsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => WordprocessingMLContentTypes.Comments;
    public override string RelationshipType => OpenXmlRelationshipTypes.Comments;
    public override string TargetName => "comments";
    public override string TargetDirectory => "";
}