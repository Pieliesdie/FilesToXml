using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(93)]
internal class StripedRightArrowType : ShapeType
{
    public StripedRightArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m@0,l@0@1,3375@1,3375@2@0@2@0,21600,21600,10800xem1350@1l1350@2,2700@2,2700@1xem0@1l0@2,675@2,675@1xe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "sum height 0 #1",
            "sum 10800 0 #1",
            "sum width 0 #0",
            "prod @4 @3 10800",
            "sum width 0 @5"
        };
        
        AdjustmentValues = "16200,5400";
        ConnectorLocations = "@0,0;0,10800;@0,21600;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "3375,@1,@6,@2";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,#1",
            xrange = "3375,21600",
            yrange = "0,10800"
        };
        
        Handles.Add(HandleOne);
    }
}