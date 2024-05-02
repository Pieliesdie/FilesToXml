using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(70)]
public class UpDownArrowType : ShapeType
{
    public UpDownArrowType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        Path = "m10800,l21600@0@3@0@3@2,21600@2,10800,21600,0@2@1@2@1@0,0@0xe";
        
        Formulas = new List<string>
        {
            "val #1",
            "val #0",
            "sum 21600 0 #1",
            "sum 21600 0 #0",
            "prod #1 #0 10800 ",
            "sum #1 0 @4",
            "sum 21600 0 @5"
        };
        
        AdjustmentValues = "5400,4320";
        
        ConnectorLocations = "10800,0;0,@0;@1,10800;0,@2;10800,21600;21600,@2;@3,10800;21600,@0";
        
        ConnectorAngles = "270,180,180,180,90,0,0,0";
        
        TextboxRectangle = "@1,@5,@3,@6";
        
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