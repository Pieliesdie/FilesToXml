using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(185)]
public class BracketPairType : ShapeType
{
    public BracketPairType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.round;
        //Encaps: Flat
        
        Path = "m@0,nfqx0@0l0@2qy@0,21600em@1,nfqx21600@0l21600@2qy@1,21600em@0,nsqx0@0l0@2qy@0,21600l@1,21600qx21600@2l21600@0qy@1,xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum width 0 #0",
            "sum height 0 #0",
            "prod @0 2929 10000",
            "sum width 0 @3",
            "sum height 0 @3",
            "val width",
            "val height",
            "prod width 1 2",
            "prod height 1 2"
        };
        
        AdjustmentValues = "3600";
        
        ConnectorLocations = "@8,0;0,@9;@8,@7;@6,@9";
        
        TextboxRectangle = "@3,@3,@4,@5";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            switchHandle = "true",
            xrange = "0,10800"
        };
        Handles.Add(HandleOne);
        
        Limo = "10800,10800";
    }
}