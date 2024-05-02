using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(98)]
public class HorizontalScrollType : ShapeType
{
    public HorizontalScrollType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "m0@5qy@2@1l@0@1@0@2qy@7,,21600@2l21600@9qy@7@10l@1@10@1@11qy@2,21600,0@11xem0@5nfqy@2@6@1@5@3@4@2@5l@2@6em@1@5nfl@1@10em21600@2nfqy@7@1l@0@1em@0@2nfqy@8@3@7@2l@7@1e";
        
        AdjustmentValues = "2700";
        ConnectorLocations = "@13,@1;0,@14;@13,@10;@12,@14";
        
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@1,@1,@7,@10";
        
        Formulas = new List<string>
        {
            "sum width 0 #0",
            "val #0 ",
            "prod @1 1 2",
            "prod @1 3 4 ",
            "prod @1 5 4 ",
            "prod @1 3 2 ",
            "prod @1 2 1 ",
            "sum width 0 @2 ",
            "sum width 0 @3 ",
            "sum height 0 @5 ",
            "sum height 0 @1 ",
            "sum height 0 @2 ",
            "val width ",
            "prod width 1 2",
            "prod height 1 2"
        };
        
        Handles = new List<Handle>();
        var handleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,5400"
        };
        
        Handles.Add(handleOne);
        Limo = "10800,10800";
    }
}