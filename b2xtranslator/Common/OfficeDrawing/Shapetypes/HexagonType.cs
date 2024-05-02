using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(9)]
public class HexagonType : ShapeType
{
    public HexagonType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@0,l,10800@0,21600@1,21600,21600,10800@1,xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum width 0 #0",
            "sum height 0 #0",
            "prod @0 2929 10000",
            "sum width 0 @3",
            "sum height 0 @3"
        };
        
        AdjustmentValues = "5400";
        
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "1800,1800,19800,19800;3600,3600,18000,18000;6300,6300,15300,15300";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}