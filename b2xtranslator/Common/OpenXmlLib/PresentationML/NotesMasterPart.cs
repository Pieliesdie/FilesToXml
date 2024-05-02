namespace b2xtranslator.OpenXmlLib.PresentationML;

public class NotesMasterPart : SlideMasterPart
{
    public NotesMasterPart(OpenXmlPartContainer parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => PresentationMLContentTypes.NotesMaster;
    public override string RelationshipType => OpenXmlRelationshipTypes.NotesMaster;
    public override string TargetName => "notesMaster" + PartIndex;
    public override string TargetDirectory => "notesMasters";
}