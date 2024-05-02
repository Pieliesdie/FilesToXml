using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(67)]
public class DownArrowType : ShapeType
{
    public DownArrowType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "m0@0l@1@0@1,0@2,0@2@0,21600@0,10800,21600xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "sum height 0 #1",
            "sum 10800 0 #1",
            "sum width 0 #0",
            "prod @4 @3 10800",
            "sum width 0 @5"
        };
        
        AdjustmentValues = "16200,5400";
        
        ConnectorLocations = "10800,0;0,@0;10800,21600;21600,@0";
        
        ConnectorAngles = "270,180,90,0";
        TextboxRectangle = "@1,0,@2,@6";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#1,#0",
            xrange = "0,10800",
            yrange = "0,21600"
        };
        Handles.Add(HandleOne);
    }
}