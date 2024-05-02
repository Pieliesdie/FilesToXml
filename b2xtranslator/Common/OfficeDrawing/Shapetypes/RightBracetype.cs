using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(88)]
public class RightBracetype : ShapeType
{
    public RightBracetype()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        
        Path = "m,qx10800@0l10800@2qy21600@11,10800@3l10800@1qy,21600e";
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
        ConnectorLocations = "0,0;21600,@11;0,21600";
        TextboxRectangle = "0,@4,7637,@5";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle();
        var HandleTwo = new Handle();
        HandleOne.position = "center,#0";
        HandleOne.yrange = "0,@8";
        HandleTwo.position = "bottomRight,#1";
        HandleTwo.yrange = "@9,@10";
        Handles.Add(HandleOne);
        Handles.Add(HandleTwo);
    }
}