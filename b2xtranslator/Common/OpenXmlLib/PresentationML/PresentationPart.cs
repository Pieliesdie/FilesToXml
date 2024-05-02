using System.Collections.Generic;

namespace b2xtranslator.OpenXmlLib.PresentationML;

public class PresentationPart : ContentPart
{
    protected static int _slideMasterCounter;
    protected static int _notesMasterCounter;
    protected static int _handoutMasterCounter;
    protected static int _slideCounter;
    protected static int _noteCounter;
    protected static int _themeCounter;
    protected static int _mediaCounter = 0;
    private readonly string _type;
    protected VbaProjectPart _vbaProjectPart;
    public List<HandoutMasterPart> HandoutMasterParts = new();
    public List<NotesMasterPart> NotesMasterParts = new();
    public List<SlideMasterPart> SlideMasterParts = new();
    
    public PresentationPart(OpenXmlPartContainer parent, string contentType)
        : base(parent, 0)
    {
        _type = contentType;
    }
    
    public override string ContentType => _type;
    public override string RelationshipType => OpenXmlRelationshipTypes.OfficeDocument;
    public override string TargetName => "presentation";
    public override string TargetDirectory => "ppt";
    
    public VbaProjectPart VbaProjectPart
    {
        get
        {
            if (_vbaProjectPart == null)
            {
                _vbaProjectPart = AddPart(new VbaProjectPart(this));
            }
            
            return _vbaProjectPart;
        }
    }
    
    public SlideMasterPart AddSlideMasterPart()
    {
        var part = new SlideMasterPart(this, ++_slideMasterCounter);
        SlideMasterParts.Add(part);
        return AddPart(part);
    }
    
    public SlideMasterPart AddNotesMasterPart()
    {
        var part = new NotesMasterPart(this, ++_notesMasterCounter);
        NotesMasterParts.Add(part);
        return AddPart(part);
    }
    
    public SlideMasterPart AddHandoutMasterPart()
    {
        var part = new HandoutMasterPart(this, ++_handoutMasterCounter);
        HandoutMasterParts.Add(part);
        return AddPart(part);
    }
    
    public SlidePart AddSlidePart()
    {
        return AddPart(new SlidePart(this, ++_slideCounter));
    }
    
    public SlidePart AddNotePart()
    {
        return AddPart(new NotePart(this, ++_noteCounter));
    }
    
    public ThemePart AddThemePart()
    {
        return AddPart(new ThemePart(this, ++_themeCounter));
    }
    
    public ViewPropertiesPart AddViewPropertiesPart()
    {
        return AddPart(new ViewPropertiesPart(this));
    }
    
    //public AppPropertiesPart AddAppPart()
    //{
    //    return this.AddPart(new AppPropertiesPart(this));
    //}
    
    //public MediaPart AddMediaPart()
    //{
    //    return this.AddPart(new MediaPart(this, ++_mediaCounter));
    //}
}