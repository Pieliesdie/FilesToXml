using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(8)]
public class TrapezoidType : ShapeType
{
    public TrapezoidType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l@0,21600@1,21600,21600,xe";
        
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
        
        ConnectorLocations = "@3,10800;10800,21600;@2,10800;10800,0";
        
        TextboxRectangle = "1800,1800,19800,19800;4500,4500,17100,17100;7200,7200,14400,14400";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,bottomRight",
            xrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}