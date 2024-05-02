using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(176)]
public class FlowChartAlternateProcessType : ShapeType
{
    public FlowChartAlternateProcessType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0,qx0@0l0@2qy@0,21600l@1,21600qx21600@2l21600@0qy@1,xe";
        
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
        
        AdjustmentValues = "2700";
        
        ConnectorLocations = "@8,0;0,@9;@8,@7;@6,@9";
        
        TextboxRectangle = "@3,@3,@4,@5";
        
        Limo = "10800,10800";
    }
}