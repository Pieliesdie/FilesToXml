using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(84)]
public class BevelType : ShapeType
{
    public BevelType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l,21600r21600,l21600,xem@0@0nfl@0@2@1@2@1@0xem,nfl@0@0em,21600nfl@0@2em21600,21600nfl@1@2em21600,nfl@1@0e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum width 0 #0",
            "sum height 0 #0",
            "prod width 1 2",
            "prod height 1 2",
            "prod #0 1 2",
            "prod #0 3 2",
            "sum @1 @5 0",
            "sum @2 @5 0"
        };
        
        AdjustmentValues = "2700";
        
        ConnectorLocations = "0,@4;@0,@4;@3,21600;@3,@2;21600,@4;@1,@4;@3,0;@3,@0";
        
        TextboxRectangle = "@0,@0,@1,@2";
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            switchHandle = "true",
            xrange = "0,10800"
        };
        Handles.Add(HandleOne);
        
        Limo = "10800,10800";
    }
}