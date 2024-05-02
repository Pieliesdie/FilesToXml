using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(80)]
internal class DownArrowCalloutType : ShapeType
{
    public DownArrowCalloutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m,l21600,,21600@0@5@0@5@2@4@2,10800,21600@1@2@3@2@3@0,0@0xe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3",
            "sum 21600 0 #1",
            "sum 21600 0 #3",
            "prod #0 1 2"
        };
        
        AdjustmentValues = "14400,5400,18000,8100";
        ConnectorLocations = "10800,0;0,@6;10800,21600;21600,@6";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "0,0,21600,@0";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,@2"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#1,bottomRight",
            xrange = "0,@3"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "#3,#2",
            xrange = "@1,10800",
            yrange = "@0,21600"
        };
        Handles.Add(HandleThree);
    }
}