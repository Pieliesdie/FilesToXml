using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(64)]
public class WaveType : ShapeType
{
    public WaveType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        Path = "m@28@0c@27@1@26@3@25@0l@21@4c@22@5@23@6@24@4xe";
        
        AdjustmentValues = "2809,10800";
        ConnectorLocations = "@35,@0;@38,10800;@37,@4;@36,10800";
        
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@31,@33,@32,@34";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod @0 41 9",
            "prod @0 23 9 ",
            "sum 0 0 @2 ",
            "sum 21600 0 #0",
            "sum 21600 0 @1 ",
            "sum 21600 0 @3 ",
            "sum #1 0 10800 ",
            "sum 21600 0 #1 ",
            "prod @8 2 3 ",
            "prod @8 4 3 ",
            "prod @8 2 1 ",
            "sum 21600 0 @9 ",
            "sum 21600 0 @10 ",
            "sum 21600 0 @11 ",
            "prod #1 2 3 ",
            "prod #1 4 3 ",
            "prod #1 2 1 ",
            "sum 21600 0 @15",
            "sum 21600 0 @16 ",
            "sum 21600 0 @17 ",
            "if @7 @14 0 ",
            "if @7 @13 @15 ",
            "if @7 @12 @16 ",
            "if @7 21600 @17 ",
            "if @7 0 @20 ",
            "if @7 @9 @19 ",
            "if @7 @10 @18 ",
            "if @7 @11 21600 ",
            "sum @24 0 @21 ",
            "sum @4 0 @0 ",
            "max @21 @25 ",
            "min @24 @28 ",
            "prod @0 2 1 ",
            "sum 21600 0 @33",
            "mid @26 @27 ",
            "mid @24 @28 ",
            "mid @22 @23 ",
            "mid @21 @25"
        };
        
        Handles = new List<Handle>();
        var handleOne = new Handle
        {
            position = "topLeft,#0",
            yrange = "0,4459"
        };
        Handles.Add(handleOne);
        
        var handleTwo = new Handle
        {
            position = "#1,bottomRight",
            xrange = "8640,12960"
        };
        Handles.Add(handleTwo);
    }
}