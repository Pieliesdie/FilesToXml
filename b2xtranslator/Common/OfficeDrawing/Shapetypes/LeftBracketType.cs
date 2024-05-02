using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(85)]
public class LeftBracketType : ShapeType
{
    public LeftBracketType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.round;
        //Endcaps: Flat
        
        Path = "m21600,qx0@0l0@1qy21600,21600e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 #0",
            "prod #0 9598 32768",
            "sum 21600 0 @2"
        };
        
        AdjustmentValues = "1800";
        ConnectorLocations = "21600,0;0,10800;21600,21600";
        TextboxRectangle = "6326,@2,21600,@3";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}