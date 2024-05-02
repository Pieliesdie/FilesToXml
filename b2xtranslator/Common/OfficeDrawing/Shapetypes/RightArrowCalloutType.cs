using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(78)]
internal class RightArrowCalloutType : ShapeType
{
    public RightArrowCalloutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m,l,21600@0,21600@0@5@2@5@2@4,21600,10800@2@1@2@3@0@3@0,x";
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
        ConnectorLocations = "@6,0;0,10800;@6,21600;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "0,0,@0,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,@2"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "bottomRight,#1",
            yrange = "0,@3"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "#2,#3",
            xrange = "@0,21600",
            yrange = "@1,10800"
        };
        Handles.Add(HandleThree);
    }
}