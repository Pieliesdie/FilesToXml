using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(61)]
public class WedgeRectCalloutType : ShapeType
{
    public WedgeRectCalloutType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path = "m,l0@8@12@24,0@9,,21600@6,21600@15@27@7,21600,21600,21600,21600@9@18@30,21600@8,21600,0@7,0@21@33@6,xe";
        
        Formulas = new List<string>
        {
            "sum 10800 0 #0",
            "sum 10800 0 #1",
            "sum #0 0 #1",
            "sum @0 @1 0",
            "sum 21600 0 #0",
            "sum 21600 0 #1",
            "if @0 3600 12600",
            "if @0 9000 18000",
            "if @1 3600 12600",
            "if @1 9000 18000",
            "if @2 0 #0",
            "if @3 @10 0",
            "if #0 0 @11",
            "if @2 @6 #0",
            "if @3 @6 @13",
            "if @5 @6 @14",
            "if @2 #0 21600",
            "if @3 21600 @16",
            "if @4 21600 @17",
            "if @2 #0 @6",
            "if @3 @19 @6",
            "if #1 @6 @20",
            "if @2 @8 #1",
            "if @3 @22 @8",
            "if #0 @8 @23",
            "if @2 21600 #1",
            "if @3 21600 @25",
            "if @5 21600 @26",
            "if @2 #1 @8",
            "if @3 @8 @28",
            "if @4 @8 @29",
            "if @2 #1 0",
            "if @3 @31 0",
            "if #1 0 @32",
            "val #0",
            "val #1"
        };
        
        AdjustmentValues = "1350,25920";
        
        ConnectorLocations = "10800,0;0,10800;10800,21600;21600,10800;@34,@35";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,#1"
        };
        Handles.Add(HandleOne);
    }
}