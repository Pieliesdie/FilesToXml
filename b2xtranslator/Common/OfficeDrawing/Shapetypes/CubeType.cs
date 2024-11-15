using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(16)]
public class CubeType : ShapeType
{
    public CubeType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0,l0@0,,21600@1,21600,21600@2,21600,xem0@0nfl@1@0,21600,em@1@0nfl@1,21600e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum width 0 #0",
            "sum height 0 #0",
            "mid height #0",
            "prod @1 1 2",
            "prod @2 1 2",
            "mid width #0"
        };
        
        AdjustmentValues = "5400";
        
        ConnectorLocations = "@6,0;@4,@0;0,@3;@4,21600;@1,@3;21600,@5";
        
        ConnectorAngles = "270,270,180,90,0,0";
        
        TextboxRectangle = "0,@0,@1,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "topLeft,#0",
            switchHandle = "true",
            yrange = "0,21600"
        };
        Handles.Add(HandleOne);
        
        Limo = "10800,10800";
    }
}