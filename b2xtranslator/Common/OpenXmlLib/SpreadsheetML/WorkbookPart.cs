using b2xtranslator.OpenXmlLib.DrawingML;

namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class WorkbookPart : OpenXmlPart
{
    private int _chartsheetNumber;
    private int _externalLinkNumber;
    protected ExternalLinkPart _externalLinkPart;
    protected SharedStringPart _sharedStringPart;
    protected StylesPart _stylesPart;
    private readonly string _type;
    protected VbaProjectPart _vbaProjectPart;
    private int _worksheetNumber;
    protected WorksheetPart _workSheetPart;
    
    public WorkbookPart(OpenXmlPartContainer parent, string contentType)
        : base(parent, 0)
    {
        _worksheetNumber = 1;
        _chartsheetNumber = 1;
        _externalLinkNumber = 1;
        _type = contentType;
    }
    
    public override string ContentType => _type;
    public override string RelationshipType => OpenXmlRelationshipTypes.OfficeDocument;
    
    /// <summary>
    ///     returns the vba project part that contains the binary macro data
    /// </summary>
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
    
    public override string TargetName => "workbook";
    public override string TargetDirectory => "xl";
    internal int DrawingsNumber { get; set; }
    
    /// <summary>
    ///     returns the newly added worksheet part from the new excel document
    /// </summary>
    /// <returns></returns>
    public WorksheetPart AddWorksheetPart()
    {
        _workSheetPart = new WorksheetPart(this, _worksheetNumber);
        _worksheetNumber++;
        return AddPart(_workSheetPart);
    }
    
    public ChartsheetPart AddChartsheetPart()
    {
        return AddPart(new ChartsheetPart(this, _chartsheetNumber++));
    }
    
    public DrawingsPart AddDrawingsPart()
    {
        return AddPart(new DrawingsPart(this, DrawingsNumber++));
    }
    
    /// <summary>
    ///     return the latest created worksheetpart
    /// </summary>
    /// <returns></returns>
    public WorksheetPart GetWorksheetPart()
    {
        return _workSheetPart;
    }
    
    /// <summary>
    ///     returns the worksheet part from the new excel document
    /// </summary>
    /// <returns></returns>
    public ExternalLinkPart AddExternalLinkPart()
    {
        _externalLinkPart = new ExternalLinkPart(this, _externalLinkNumber);
        _externalLinkNumber++;
        return AddPart(_externalLinkPart);
    }
    
    /// <summary>
    ///     return the latest created worksheetpart
    /// </summary>
    /// <returns></returns>
    public ExternalLinkPart GetExternalLinkPart()
    {
        return _externalLinkPart;
    }
    
    /// <summary>
    ///     returns the sharedstringtable part from the new excel document
    /// </summary>
    /// <returns></returns>
    public SharedStringPart AddSharedStringPart()
    {
        _sharedStringPart = new SharedStringPart(this);
        return AddPart(_sharedStringPart);
    }
    
    /// <summary>
    ///     returns the sharedstringtable part from the new excel document
    /// </summary>
    /// <returns></returns>
    public StylesPart AddStylesPart()
    {
        _stylesPart = new StylesPart(this);
        return AddPart(_stylesPart);
    }
}