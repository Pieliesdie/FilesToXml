using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(58)]
internal class Seal8Type : ShapeType
{
    public Seal8Type()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m21600,10800l@3@6,18436,3163@4@5,10800,0@6@5,3163,3163@5@6,,10800@5@4,3163,18436@6@3,10800,21600@4@3,18436,18436@3@4xe";
        
        Formulas = new List<string>
        {
            "sum 10800 0 #0",
            "prod @0 30274 32768",
            "prod @0 12540 32768",
            "sum @1 10800 0",
            "sum @2 10800 0",
            "sum 10800 0 @1",
            "sum 10800 0 @2",
            "prod @0 23170 32768",
            "sum @7 10800 0",
            "sum 10800 0 @7"
        };
        
        AdjustmentValues = "2538";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "@9,@9,@8,@8";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "0,10800"
        };
        
        Handles.Add(HandleOne);
    }
}