using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(172)]
public class TextSlantUp : ShapeType
{
    public TextSlantUp()
    {
        TextPath = true;
        
        Joins = JoinStyle.none;
        
        AdjustmentValues = "12000";
        
        Path = "m0@0l21600,m,21600l21600@1e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 @0",
            "prod #0 1 2",
            "sum @2 10800 0",
            "prod @1 1 2",
            "sum @4 10800 0"
        };
        
        ConnectorLocations = "10800,@2;0,@3;10800,@5;21600,@4";
        ConnectorAngles = "270,180,90,0";
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,15429"
        };
        Handles.Add(h1);
    }
}