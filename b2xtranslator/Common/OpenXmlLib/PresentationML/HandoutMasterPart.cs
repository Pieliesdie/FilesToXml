namespace b2xtranslator.OpenXmlLib.PresentationML;

public class HandoutMasterPart : SlideMasterPart
{
    public HandoutMasterPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.HandoutMaster;
    public override string RelationshipType => OpenXmlRelationshipTypes.HandoutMaster;
    public override string TargetName => "handoutMaster" + PartIndex;
    public override string TargetDirectory => "handoutMasters";
}