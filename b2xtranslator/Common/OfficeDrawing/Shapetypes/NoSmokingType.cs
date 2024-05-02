using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(57)]
internal class NoSmokingType : ShapeType
{
    public NoSmokingType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m,10800qy10800,,21600,10800,10800,21600,,10800xar@0@0@16@16@12@14@15@13xar@0@0@16@16@13@15@14@12xe";
        Formulas = new List<string>
        {
            "val #0",
            "prod @0 2 1",
            "sum 21600 0 @1",
            "prod @2 @2 1 ",
            "prod @0 @0 1",
            "sum @3 0 @4",
            "prod @5 1 8 ",
            "sqrt @6 ",
            "prod @4 1 8 ",
            "sqrt @8 ",
            "sum @7 @9 0",
            "sum @7 0 @9",
            "sum @10 10800 0",
            "sum 10800 0 @10",
            "sum @11 10800 0 ",
            "sum 10800 0 @11 ",
            "sum 21600 0 @0"
        };
        
        AdjustmentValues = "2700";
        ConnectorLocations = "10800,0;3163,3163;0,10800;3163,18437;10800,21600;18437,18437;21600,10800;18437,3163";
        
        TextboxRectangle = "3163,3163,18437,18437";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "0,7200"
        };
        Handles.Add(HandleOne);
    }
}