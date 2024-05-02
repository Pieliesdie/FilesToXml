namespace b2xtranslator.OpenXmlLib.PresentationML;

public class PresentationDocument : OpenXmlPackage
{
    protected DocumentType _documentType;
    protected PresentationPart _presentationPart;
    
    protected PresentationDocument(string fileName, DocumentType type)
        : base(fileName)
    {
        switch (type)
        {
            case DocumentType.Document:
                _presentationPart = new PresentationPart(this, PresentationMLContentTypes.Presentation);
                break;
            case DocumentType.MacroEnabledDocument:
                _presentationPart = new PresentationPart(this, PresentationMLContentTypes.PresentationMacro);
                break;
            case DocumentType.Template:
                break;
            case DocumentType.MacroEnabledTemplate:
                break;
        }
        
        AddPart(_presentationPart);
    }
    
    public PresentationPart PresentationPart => _presentationPart;
    
    public static PresentationDocument Create(string fileName, DocumentType type)
    {
        var presentation = new PresentationDocument(fileName, type);
        
        return presentation;
    }
}