using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(96)]
public class SmileyFaceType : ShapeType
{
    public SmileyFaceType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.round;
        
        Path = "m10800,qx,10800,10800,21600,21600,10800,10800,xem7340,6445qx6215,7570,7340,8695,8465,7570,7340,6445xnfem14260,6445qx13135,7570,14260,8695,15385,7570,14260,6445xnfem4960@0c8853@3,12747@3,16640@0nfe";
        
        Formulas = new List<string>
        {
            "sum 33030 0 #0",
            "prod #0 4 3",
            "prod @0 1 3",
            "sum @1 0 @2"
        };
        
        AdjustmentValues = "17520";
        
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163";
        
        TextboxRectangle = "3163,3163,18437,18437";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "center,#0",
            yrange = "15510,17520"
        };
        Handles.Add(HandleOne);
    }
}