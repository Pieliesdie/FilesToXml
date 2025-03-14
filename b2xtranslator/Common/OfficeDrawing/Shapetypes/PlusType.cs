using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(11)]
public class PlusType : ShapeType
{
    public PlusType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0,l@0@0,0@0,0@2@0@2@0,21600@1,21600@1@2,21600@2,21600@0@1@0@1,xe";
        
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
        
        AdjustmentValues = "5400";
        
        ConnectorLocations = "@8,0;0,@9;@8,@7;@6,@9";
        
        TextboxRectangle = "0,0,21600,21600;5400,5400,16200,16200;10800,10800,10800,10800";
        
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