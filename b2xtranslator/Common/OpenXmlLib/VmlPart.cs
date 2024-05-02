namespace b2xtranslator.OpenXmlLib;

public class VmlPart : ContentPart
{
    internal VmlPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => "application/vnd.openxmlformats-officedocument.vmlDrawing";
    internal override bool HasDefaultContentType => true;
    public override string RelationshipType => OpenXmlRelationshipTypes.Vml;
    public override string TargetName => "vmlDrawing" + PartIndex;
    public override string TargetDirectory { get; set; } = "drawings";
    public override string TargetExt => ".vml";
}