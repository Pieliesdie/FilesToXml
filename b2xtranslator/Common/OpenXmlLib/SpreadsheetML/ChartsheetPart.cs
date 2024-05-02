using b2xtranslator.OpenXmlLib.DrawingML;

namespace b2xtranslator.OpenXmlLib.SpreadsheetML;

public class ChartsheetPart : OpenXmlPart
{
    private DrawingsPart _drawingsPart;
    
    public ChartsheetPart(WorkbookPart parent, int partIndex)
        : base(parent, partIndex) { }
    
    public override string ContentType => SpreadsheetMLContentTypes.Chartsheet;
    public override string RelationshipType => OpenXmlRelationshipTypes.Chartsheet;
    public override string TargetName => "sheet" + PartIndex;
    public override string TargetDirectory => "chartsheets";
    
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