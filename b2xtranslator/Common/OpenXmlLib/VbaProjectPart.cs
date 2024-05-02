namespace b2xtranslator.OpenXmlLib;

public class VbaProjectPart : ContentPart
{
    protected VbaDataPart _vbaDataPart;
    
    internal VbaProjectPart(OpenXmlPartContainer parent)
        : base(parent, 0) { }
    
    public override string ContentType => MicrosoftWordContentTypes.VbaProject;
    public override string RelationshipType => MicrosoftWordRelationshipTypes.VbaProject;
    public override string TargetName => "vbaProject";
    public override string TargetExt => ".bin";
    public override string TargetDirectory => "";
    
    public VbaDataPart VbaDataPart
    {
        get
        {
            if (_vbaDataPart == null)
            {
                _vbaDataPart = AddPart(new VbaDataPart(this));
            }
            
            return _vbaDataPart;
        }
    }
}