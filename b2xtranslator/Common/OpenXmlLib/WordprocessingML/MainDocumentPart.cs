namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class MainDocumentPart : ContentPart
{
    protected CommentsPart _commentsPart;
    private readonly string _contentType = WordprocessingMLContentTypes.MainDocument;
    protected KeyMapCustomizationsPart _customizationsPart;
    protected EndnotesPart _endnotesPart;
    protected FontTablePart _fontTablePart;
    protected int _footerPartCount;
    protected FootnotesPart _footnotesPart;
    protected GlossaryPart _glossaryPart;
    protected int _headerPartCount;
    protected NumberingDefinitionsPart _numberingDefinitionsPart;
    protected SettingsPart _settingsPart;
    protected StyleDefinitionsPart _styleDefinitionsPart;
    protected VbaProjectPart _vbaProjectPart;
    
    public MainDocumentPart(OpenXmlPartContainer parent, string contentType)
        : base(parent)
    {
        _contentType = contentType;
    }
    
    public override string ContentType => _contentType;
    public override string RelationshipType => OpenXmlRelationshipTypes.OfficeDocument;
    public override string TargetName => "document";
    public override string TargetDirectory => "word";
    
    // unique parts
    
    public KeyMapCustomizationsPart CustomizationsPart
    {
        get
        {
            if (_customizationsPart == null)
            {
                _customizationsPart = new KeyMapCustomizationsPart(this);
                AddPart(_customizationsPart);
            }
            
            return _customizationsPart;
        }
    }
    
    public GlossaryPart GlossaryPart
    {
        get
        {
            if (_glossaryPart == null)
            {
                _glossaryPart = new GlossaryPart(this, WordprocessingMLContentTypes.Glossary);
                AddPart(_glossaryPart);
            }
            
            return _glossaryPart;
        }
    }
    
    public StyleDefinitionsPart StyleDefinitionsPart
    {
        get
        {
            if (_styleDefinitionsPart == null)
            {
                _styleDefinitionsPart = new StyleDefinitionsPart(this);
                AddPart(_styleDefinitionsPart);
            }
            
            return _styleDefinitionsPart;
        }
    }
    
    public SettingsPart SettingsPart
    {
        get
        {
            if (_settingsPart == null)
            {
                _settingsPart = new SettingsPart(this);
                AddPart(_settingsPart);
            }
            
            return _settingsPart;
        }
    }
    
    public NumberingDefinitionsPart NumberingDefinitionsPart
    {
        get
        {
            if (_numberingDefinitionsPart == null)
            {
                _numberingDefinitionsPart = new NumberingDefinitionsPart(this);
                AddPart(_numberingDefinitionsPart);
            }
            
            return _numberingDefinitionsPart;
        }
    }
    
    public FontTablePart FontTablePart
    {
        get
        {
            if (_fontTablePart == null)
            {
                _fontTablePart = new FontTablePart(this);
                AddPart(_fontTablePart);
            }
            
            return _fontTablePart;
        }
    }
    
    public EndnotesPart EndnotesPart
    {
        get
        {
            if (_endnotesPart == null)
            {
                _endnotesPart = new EndnotesPart(this);
                AddPart(_endnotesPart);
            }
            
            return _endnotesPart;
        }
    }
    
    public FootnotesPart FootnotesPart
    {
        get
        {
            if (_footnotesPart == null)
            {
                _footnotesPart = new FootnotesPart(this);
                AddPart(_footnotesPart);
            }
            
            return _footnotesPart;
        }
    }
    
    public CommentsPart CommentsPart
    {
        get
        {
            if (_commentsPart == null)
            {
                _commentsPart = new CommentsPart(this);
                AddPart(_commentsPart);
            }
            
            return _commentsPart;
        }
    }
    
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
    
    // non unique parts
    
    public HeaderPart AddHeaderPart()
    {
        return AddPart(new HeaderPart(this, ++_headerPartCount));
    }
    
    public FooterPart AddFooterPart()
    {
        return AddPart(new FooterPart(this, ++_footerPartCount));
    }
}