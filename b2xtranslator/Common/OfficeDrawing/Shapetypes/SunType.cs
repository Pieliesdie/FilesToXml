using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(183)]
internal class SunType : ShapeType
{
    public SunType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path =
            "m21600,10800l@15@14@15@18xem18436,3163l@17@12@16@13xem10800,l@14@10@18@10xem3163,3163l@12@13@13@12xem,10800l@10@18@10@14xem3163,18436l@13@16@12@17xem10800,21600l@18@15@14@15xem18436,18436l@16@17@17@16xem10800@19qx@19,10800,10800@20@20,10800,10800@19xe";
        Formulas = new List<string>
        {
            "sum 10800 0 #0 ",
            "prod @0 30274 32768 ",
            "prod @0 12540 32768 ",
            "sum @1 10800 0 ",
            "sum @2 10800 0 ",
            "sum 10800 0 @1 ",
            "sum 10800 0 @2 ",
            "prod @0 23170 32768 ",
            "sum @7 10800 0 ",
            "sum 10800 0 @7 ",
            "prod @5 3 4 ",
            "prod @6 3 4 ",
            "sum @10 791 0 ",
            "sum @11 791 0 ",
            "sum @11 2700 0",
            "sum 21600 0 @10 ",
            "sum 21600 0 @12 ",
            "sum 21600 0 @13 ",
            "sum 21600 0 @14 ",
            "val #0 ",
            "sum 21600 0 #0"
        };
        
        AdjustmentValues = "5400";
        TextboxRectangle = "@9,@9,@8,@8";
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "2700,10125"
        };
        Handles.Add(HandleOne);
    }
}