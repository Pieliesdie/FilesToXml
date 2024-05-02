using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(44)]
public class AccentCallout1Type : ShapeType
{
    public AccentCallout1Type()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0@1l@2@3nfem@2,l@2,21600nfem,l21600,r,21600l,21600nsxe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "val #3"
        };
        AdjustmentValues = "8280,24300,-1800,4050";
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