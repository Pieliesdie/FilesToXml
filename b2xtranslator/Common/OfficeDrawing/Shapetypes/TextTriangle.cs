using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(138)]
public class TextTriangle : ShapeType
{
    public TextTriangle()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        
        AdjustmentValues = "10800";
        Path = "m0@0l10800,,21600@0m,21600r10800,l21600,21600e";
        ConnectorLocations = "10800,0;5400,@1;10800,21600;16200,@1";
        ConnectorAngles = "270,180,90,0";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 1 2",
            "sum @1 10800 0",
            "sum 21600 0 @1"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,21600"
        };
        Handles.Add(h1);
    }
}