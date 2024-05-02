using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(89)]
internal class LeftUpArrowType : ShapeType
{
    public LeftUpArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m@4,l@0@2@5@2@5@5@2@5@2@0,0@4@2,21600@2@1@1@1@1@2,21600@2xe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "prod #0 1 2",
            "sum @3 10800 0",
            "sum 21600 #0 #1",
            "sum #1 #2 0",
            "prod @6 1 2",
            "prod #1 2 1",
            "sum @8 0 21600",
            "sum @5 0 @4",
            "sum #0 0 @4",
            "prod @2 @10 @11"
        };
        
        AdjustmentValues = "9257,18514,6171";
        ConnectorLocations = "@4,0;@0,@2;@2,@0;0,@4;@2,21600;@7,@1;@1,@7;21600,@2";
        ConnectorAngles = "270,180,270,180,90,90,0,0";
        
        TextboxRectangle = "@12,@5,@1,@1;@5,@12,@1,@1";
        
        Handles = new List<Handle>();
        
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "@2,@9"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#1,#2",
            xrange = "@4,21600",
            yrange = "0,@0"
        };
        Handles.Add(HandleTwo);
    }
}