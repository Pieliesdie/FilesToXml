using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(181)]
public class AccentBorderCallout90Type : ShapeType
{
    public AccentBorderCallout90Type()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0@1l@2@3nfem,l21600,r,21600l,21600xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3"
        };
        
        AdjustmentValues = "-1800,24300,-1800,4050";
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
    }
}