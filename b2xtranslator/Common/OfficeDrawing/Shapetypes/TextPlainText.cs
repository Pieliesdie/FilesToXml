using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(136)]
public class TextPlainText : ShapeType
{
    public TextPlainText()
    {
        TextPath = true;
        
        Joins = JoinStyle.none;
        
        AdjustmentValues = "10800";
        
        Path = "m@7,l@8,m@5,21600l@6,21600e";
        
        Formulas = new List<string>
        {
            "sum #0 0 10800",
            "prod #0 2 1",
            "sum 21600 0 @1",
            "sum 0 0 @2",
            "sum 21600 0 @3",
            "if @0 @3 0",
            "if @0 21600 @1",
            "if @0 0 @2",
            "if @0 @4 21600",
            "mid @5 @6",
            "mid @8 @5",
            "mid @7 @8",
            "mid @6 @7",
            "sum @6 0 @5"
        };
        
        ConnectorLocations = "@9,0;@10,10800;@11,21600;@12,10800";
        ConnectorAngles = "270,180,90,0";
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "#0,bottomRight",
            xrange = "6629,14971"
        };
        Handles.Add(h1);
    }
}