using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(22)]
public class CanType : ShapeType
{
    public CanType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.round;
        
        Path = "m10800,qx0@1l0@2qy10800,21600,21600@2l21600@1qy10800,xem0@1qy10800@0,21600@1nfe";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 1 2",
            "sum height 0 @1"
        };
        
        AdjustmentValues = "5400";
        
        ConnectorLocations = "10800,@0;10800,0;0,10800;10800,21600;21600,10800";
        
        ConnectorAngles = "270,270,180,90,0";
        
        TextboxRectangle = "0,@0,21600,@2";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "center,#0",
            yrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}