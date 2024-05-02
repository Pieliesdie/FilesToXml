using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(82)]
internal class UpDownArrowCalloutType : ShapeType
{
    public UpDownArrowCalloutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m0@0l@3@0@3@2@1@2,10800,0@4@2@5@2@5@0,21600@0,21600@8@5@8@5@9@4@9,10800,21600@1@9@3@9@3@8,0@8xe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3",
            "sum 21600 0 #1",
            "sum 21600 0 #3",
            "sum #0 21600 0",
            "prod @6 1 2",
            "sum 21600 0 #0",
            "sum 21600 0 #2"
        };
        
        AdjustmentValues = "5400,5400,2700,8100";
        ConnectorLocations = "10800,0;0,10800;10800,21600;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "0,@0,21600,@8";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "@2,10800"
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