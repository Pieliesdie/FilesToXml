namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

/// <summary>
///     Includes some information about the spreadsheetdocument
/// </summary>
public class SpreadsheetDocument : OpenXmlPackage
{
    protected DocumentType _documentType;
    protected WorkbookPart workBookPart;
    
    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="fileName">Filename of the file which should be written</param>
    protected SpreadsheetDocument(string fileName, DocumentType type)
        : base(fileName)
    {
        switch (type)
        {
            case DocumentType.Document:
                workBookPart = new WorkbookPart(this, SpreadsheetMLContentTypes.Workbook);
                break;
            case DocumentType.MacroEnabledDocument:
                workBookPart = new WorkbookPart(this, SpreadsheetMLContentTypes.WorkbookMacro);
                break;
            //case OpenXmlPackage.DocumentType.Template:
            //    workBookPart = new WorkbookPart(this, WordprocessingMLContentTypes.MainDocumentTemplate);
            //    break;
            //case OpenXmlPackage.DocumentType.MacroEnabledTemplate:
            //    workBookPart = new WorkbookPart(this, WordprocessingMLContentTypes.MainDocumentMacroTemplate);
            //    break;
        }
        
        _documentType = type;
        AddPart(workBookPart);
    }
    
    public DocumentType DocumentType
    {
        get => _documentType;
        set => _documentType = value;
    }
    
    /// <summary>
    ///     returns the workbookPart from the new excel document
    /// </summary>
    public WorkbookPart WorkbookPart => workBookPart;
    
    /// <summary>
    ///     creates a new excel document with the choosen filename
    /// </summary>
    /// <param name="fileName">The name of the file which should be written</param>
    /// <returns>The object itself</returns>
    public static SpreadsheetDocument Create(string fileName, DocumentType type)
    {
        var spreadsheet = new SpreadsheetDocument(fileName, type);
        return spreadsheet;
    }
}