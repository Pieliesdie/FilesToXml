using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(170)]
public class TextFadeUp : ShapeType
{
    public TextFadeUp()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "7200";
        Path = "m@0,l@1,m,21600r21600,e";
        ConnectorLocations = "10800,0;@2,10800;10800,21600;@3,10800";
        ConnectorAngles = "270,180,90,0";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 @0",
            "prod #0 1 2",
            "sum 21600 0 @2",
            "sum @1 21600 @0"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,10792"
        };
        Handles.Add(h1);
    }
}