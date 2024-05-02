using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(23)]
internal class DonutType : ShapeType
{
    public DonutType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.round;
        Path = "m,10800qy10800,,21600,10800,10800,21600,,10800xm@0,10800qy10800@2@1,10800,10800@0@0,10800xe";
        
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
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163";
        TextboxRectangle = "3163,3163,18437,18437";
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "0,10800"
        };
        Handles.Add(HandleOne);
    }
}