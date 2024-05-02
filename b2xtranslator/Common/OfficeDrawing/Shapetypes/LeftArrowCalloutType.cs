using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(77)]
internal class LeftArrowCalloutType : ShapeType
{
    public LeftArrowCalloutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m@0,l@0@3@2@3@2@1,,10800@2@4@2@5@0@5@0,21600,21600,21600,21600,xe";
        Formulas = new List<string>
        {
            "val #0 ",
            "val #1 ",
            "val #2 ",
            "val #3 ",
            "sum 21600 0 #1",
            "sum 21600 0 #3",
            "sum #0 21600 0"
        };
        
        AdjustmentValues = "7200,5400,3600,8100";
        ConnectorLocations = "@7,0;0,10800;@7,21600;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@0,0,21600,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "@2,21600"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "topLeft,#1",
            yrange = "0,@3"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "#2,#3",
            xrange = "0,@0",
            yrange = "@1,10800"
        };
        Handles.Add(HandleThree);
    }
}