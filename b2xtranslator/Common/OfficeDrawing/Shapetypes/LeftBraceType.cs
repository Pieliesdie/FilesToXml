using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(87)]
public class LeftBraceType : ShapeType
{
    public LeftBraceType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        //Endcaps: Flat
        
        Path = "m21600,qx10800@0l10800@2qy0@11,10800@3l10800@1qy21600,21600e";
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 #0",
            "sum #1 0 #0",
            "sum #1 #0 0",
            "prod #0 9598 32768",
            "sum 21600 0 @4",
            "sum 21600 0 #1",
            "min #1 @6",
            "prod @7 1 2",
            "prod #0 2 1",
            "sum 21600 0 @9",
            "val #1"
        };
        
        AdjustmentValues = "1800,10800";
        ConnectorLocations = "21600,0;0,10800;21600,21600";
        TextboxRectangle = "13963,@4,21600,@5";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle();
        var HandleTwo = new Handle();
        HandleOne.position = "center,#0";
        HandleOne.yrange = "0,@8";
        HandleTwo.position = "topLeft,#1";
        HandleTwo.yrange = "@9,@10";
        Handles.Add(HandleOne);
        Handles.Add(HandleTwo);
    }
}