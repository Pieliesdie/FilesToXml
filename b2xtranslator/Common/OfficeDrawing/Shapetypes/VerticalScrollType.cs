using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(97)]
public class VerticalScrollType : ShapeType
{
    public VerticalScrollType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "m@5,qx@1@2l@1@0@2@0qx0@7@2,21600l@9,21600qx@10@7l@10@1@11@1qx21600@2@11,xem@5,nfqx@6@2@5@1@4@3@5@2l@6@2em@5@1nfl@10@1em@2,21600nfqx@1@7l@1@0em@2@0nfqx@3@8@2@7l@1@7e";
        
        AdjustmentValues = "2700";
        ConnectorLocations = "@14,0;@1,@13;@14,@12;@10,@13";
        
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@1,@1,@10,@7";
        
        Formulas = new List<string>
        {
            "sum height 0 #0 ",
            "val #0 ",
            "prod @1 1 2 ",
            "prod @1 3 4 ",
            "prod @1 5 4 ",
            "prod @1 3 2 ",
            "prod @1 2 1 ",
            "sum height 0 @2 ",
            "sum height 0 @3 ",
            "sum width 0 @5 ",
            "sum width 0 @1 ",
            "sum width 0 @2",
            "val height ",
            "prod height 1 2",
            "prod width 1 2"
        };
        
        Handles = new List<Handle>();
        var handleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,5400"
        };
        
        Handles.Add(handleOne);
        Limo = "10800,10800";
    }
}