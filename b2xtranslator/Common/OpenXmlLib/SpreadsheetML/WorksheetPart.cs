using b2xtranslator.OpenXmlLib.DrawingML;

namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class WorksheetPart : OpenXmlPart
{
    private DrawingsPart _drawingsPart;
    
    public WorksheetPart(WorkbookPart parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => SpreadsheetMLContentTypes.Worksheet;
    public override string RelationshipType => OpenXmlRelationshipTypes.Worksheet;
    public override string TargetName => "sheet" + PartIndex;
    public override string TargetDirectory => "worksheets";
    
    public DrawingsPart DrawingsPart
    {
        get
        {
            if (_drawingsPart == null)
            {
                _drawingsPart = AddPart(new DrawingsPart(this, ++((WorkbookPart)Parent).DrawingsNumber));
                //this._drawingsPart = ((WorkbookPart)this.Parent).AddDrawingsPart();
            }
            
            return _drawingsPart;
        }
    }
}