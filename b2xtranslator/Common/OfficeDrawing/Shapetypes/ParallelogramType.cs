using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(7)]
public class ParallelogramType : ShapeType
{
    public ParallelogramType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0,l,21600@1,21600,21600,xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum width 0 #0",
            "prod #0 1 2",
            "sum width 0 @2",
            "mid #0 width",
            "mid @1 0",
            "prod height width #0",
            "prod @6 1 2",
            "sum height 0 @7",
            "prod width 1 2",
            "sum #0 0 @9",
            "if @10 @8 0",
            "if @10 @7 height"
        };
        
        AdjustmentValues = "5400";
        
        ConnectorLocations = "@4,0;10800,@11;@3,10800;@5,21600;10800,@12;@2,10800";
        
        TextboxRectangle = "1800,1800,19800,19800;8100,8100,13500,13500;10800,10800,10800,10800";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,21600"
        };
        Handles.Add(HandleOne);
    }
}