using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(182)]
internal class LeftRightUpArrow : ShapeType
{
    public LeftRightUpArrow()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m10800,l@0@2@1@2@1@6@7@6@7@5,0@8@7,21600@7@9@10@9@10,21600,21600@8@10@5@10@6@4@6@4@2@3@2xe";
        Formulas = new List<string>
        {
            "val #0 ",
            "val #1 ",
            "val #2 ",
            "sum 21600 0 #0",
            "sum 21600 0 #1",
            "prod @0 21600 @3 ",
            "prod @1 21600 @3 ",
            "prod @2 @3 21600 ",
            "prod 10800 21600 @3 ",
            "prod @4 21600 @3 ",
            "sum 21600 0 @7 ",
            "sum @5 0 @8 ",
            "sum @6 0 @8 ",
            "prod @12 @7 @11 ",
            "sum 21600 0 @13 ",
            "sum @0 0 10800 ",
            "sum @1 0 10800 ",
            "prod @2 @16 @15"
        };
        
        AdjustmentValues = "6480,8640,6171";
        ConnectorLocations = "10800,0;0,@8;10800,@9;21600,@8";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@13,@6,@14,@9;@1,@17,@4,@9";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "@2,@1"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#1,#2",
            xrange = "@0,10800",
            yrange = "0,@5"
        };
        Handles.Add(HandleTwo);
    }
}