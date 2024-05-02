using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(79)]
internal class UpArrowCalloutType : ShapeType
{
    public UpArrowCalloutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m0@0l@3@0@3@2@1@2,10800,0@4@2@5@2@5@0,21600@0,21600,21600,,21600xe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3",
            "sum 21600 0 #1",
            "sum 21600 0 #3",
            "sum #0 21600 0",
            "prod @6 1 2"
        };
        
        AdjustmentValues = "7200,5400,3600,8100";
        ConnectorLocations = "10800,0;0,@7;10800,21600;21600,@7";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "0,@0,21600,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "@2,21600"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#1,topLeft",
            xrange = "0,@3"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "#3,#2",
            xrange = "@1,10800",
            yrange = "0,@0"
        };
        Handles.Add(HandleThree);
    }
}