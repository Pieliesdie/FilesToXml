namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class ToolbarsPart : ContentPart
{
    internal ToolbarsPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => MicrosoftWordContentTypes.Toolbars;
    public override string RelationshipType => MicrosoftWordRelationshipTypes.Toolbars;
    public override string TargetName => "attachedToolbars";
    public override string TargetExt => ".bin";
    public override string TargetDirectory => "";
}