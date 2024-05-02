namespace b2xtranslator.OpenXmlLib.WordprocessingML;

public class WordprocessingDocument : OpenXmlPackage
{
    protected CustomXmlPropertiesPart _customFilePropertiesPart;
    protected DocumentType _documentType;
    protected MainDocumentPart _mainDocumentPart;
    
    protected WordprocessingDocument(string fileName, DocumentType type)
        : base(fileName)
    {
        switch (type)
        {
            case DocumentType.Document:
                _mainDocumentPart = new MainDocumentPart(this, WordprocessingMLContentTypes.MainDocument);
                break;
            case DocumentType.MacroEnabledDocument:
                _mainDocumentPart = new MainDocumentPart(this, WordprocessingMLContentTypes.MainDocumentMacro);
                break;
            case DocumentType.Template:
                _mainDocumentPart = new MainDocumentPart(this, WordprocessingMLContentTypes.MainDocumentTemplate);
                break;
            case DocumentType.MacroEnabledTemplate:
                _mainDocumentPart = new MainDocumentPart(this, WordprocessingMLContentTypes.MainDocumentMacroTemplate);
                break;
        }
        
        _documentType = type;
        AddPart(_mainDocumentPart);
    }
    
    public DocumentType DocumentType
    {
        get => _documentType;
        set => _documentType = value;
    }
    
    public CustomXmlPropertiesPart CustomFilePropertiesPart => _customFilePropertiesPart;
    public MainDocumentPart MainDocumentPart => _mainDocumentPart;
    
    public static WordprocessingDocument Create(string fileName, DocumentType type)
    {
        var doc = new WordprocessingDocument(fileName, type);
        
        return doc;
    }
}