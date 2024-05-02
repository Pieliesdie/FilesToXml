using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(152)]
public class TextCurveUp : ShapeType
{
    public TextCurveUp()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "9931";
        Path = "m0@0c7200@2,14400@1,21600,m0@5c7200@6,14400@6,21600@5e";
        ConnectorLocations = "10800,@10;0,@9;10800,21600;21600,@8";
        ConnectorAngles = "270,180,90,0";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 3 4",
            "prod #0 5 4",
            "prod #0 3 8",
            "prod #0 1 8",
            "sum 21600 0 @3",
            "sum @4 21600 0",
            "prod #0 1 2",
            "prod @5 1 2",
            "sum @7 @8 0",
            "prod #0 7 8",
            "prod @5 1 3",
            "sum @1 @2 0",
            "sum @12 @0 0",
            "prod @13 1 4",
            "sum @11 14400 @14"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,12169"
        };
        Handles.Add(h1);
    }
}