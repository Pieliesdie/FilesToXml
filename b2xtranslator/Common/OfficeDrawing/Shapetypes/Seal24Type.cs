using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(92)]
internal class Seal24Type : ShapeType
{
    public Seal24Type()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path =
            "m21600,10800l@7@14,21232,8005@9@16,20153,5400@11@18,18437,3163@12@17,16200,1447@10@15,13595,368@8@13,10800,0@14@13,8005,368@16@15,5400,1447@18@17,3163,3163@17@18,1447,5400@15@16,368,8005@13@14,,10800@13@8,368,13595@15@10,1447,16200@17@12,3163,18437@18@11,5400,20153@16@9,8005,21232@14@7,10800,21600@8@7,13595,21232@10@9,16200,20153@12@11,18437,18437@11@12,20153,16200@9@10,21232,13595@7@8xe";
        
        Formulas = new List<string>
        {
            "sum 10800 0 #0",
            "prod @0 32488 32768",
            "prod @0 4277 32768",
            "prod @0 30274 32768",
            "prod @0 12540 32768",
            "prod @0 25997 32768",
            "prod @0 19948 32768",
            "sum @1 10800 0",
            "sum @2 10800 0",
            "sum @3 10800 0",
            "sum @4 10800 0",
            "sum @5 10800 0",
            "sum @6 10800 0",
            "sum 10800 0 @1",
            "sum 10800 0 @2",
            "sum 10800 0 @3",
            "sum 10800 0 @4",
            "sum 10800 0 @5",
            "sum 10800 0 @6",
            "prod @0 23170 32768",
            "sum @19 10800 0",
            "sum 10800 0 @19"
        };
        
        AdjustmentValues = "2700";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "@21,@21,@20,@20";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "0,10800"
        };
        
        Handles.Add(HandleOne);
    }
}