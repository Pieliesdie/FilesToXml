using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(69)]
public class LeftRightArrowType : ShapeType
{
    public LeftRightArrowType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "m,10800l@0,21600@0@3@2@3@2,21600,21600,10800@2,0@2@1@0@1@0,xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "sum 21600 0 #0",
            "sum 21600 0 #1",
            "prod #0 #1 10800",
            "sum #0 0 @4",
            "sum 21600 0 @5"
        };
        
        AdjustmentValues = "4320,5400";
        
        ConnectorLocations = "@2,0;10800,@1;@0,0;0,10800;@0,21600;10800,@3;@2,21600;21600,10800";
        
        ConnectorAngles = "270,270,270,180,90,90,90,0";
        
        TextboxRectangle = "@5,@1,@6,@3";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,#1",
            xrange = "0,10800",
            yrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}