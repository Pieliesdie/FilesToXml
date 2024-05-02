using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(86)]
public class RightBracketType : ShapeType
{
    public RightBracketType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.round;
        //Endcaps: Flat
        
        Path = "m,qx21600@0l21600@1qy,21600e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 #0",
            "prod #0 9598 32768",
            "sum 21600 0 @2"
        };
        
        AdjustmentValues = "1800";
        ConnectorLocations = "0,0;0,21600;21600,10800";
        TextboxRectangle = "0,@2,15274,@3";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "bottomRight,#0",
            yrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}