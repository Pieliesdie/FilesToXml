using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(49)]
public class BorderCallout3Type : ShapeType
{
    public BorderCallout3Type()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        Path = "m@0@1l@2@3@4@5@6@7nfem,l21600,r,21600l,21600xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3",
            "val #4",
            "val #5",
            "val #6",
            "val #7"
        };
        AdjustmentValues = "23400,24400,25200,21600,25200,4050,23400,4050";
        ConnectorLocations = "@0,@1;10800,0;10800,21600;0,10800;21600,10800";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,#1"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#2,#3"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "#4,#5"
        };
        Handles.Add(HandleThree);
        
        var HandleFour = new Handle
        {
            position = "#6,#7"
        };
        Handles.Add(HandleFour);
    }
}