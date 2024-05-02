using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(63)]
public class WedgeEllipseCalloutType : ShapeType
{
    public WedgeEllipseCalloutType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "wr,,21600,21600@15@16@17@18l@21@22xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "sum 10800 0 #0",
            "sum 10800 0 #1",
            "atan2 @2 @3",
            "sumangle @4 11 0",
            "sumangle @4 0 11",
            "cos 10800 @4",
            "sin 10800 @4",
            "cos 10800 @5",
            "sin 10800 @5",
            "cos 10800 @6",
            "sin 10800 @6",
            "sum 10800 0 @7",
            "sum 10800 0 @8",
            "sum 10800 0 @9",
            "sum 10800 0 @10",
            "sum 10800 0 @11",
            "sum 10800 0 @12",
            "mod @2 @3 0",
            "sum @19 0 10800",
            "if @20 #0 @13",
            "if @20 #1 @14"
        };
        
        AdjustmentValues = "1350,25920";
        
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163;@21,@22";
        
        TextboxRectangle = "3163,3163,18437,18437";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,#1"
        };
        Handles.Add(HandleOne);
    }
}