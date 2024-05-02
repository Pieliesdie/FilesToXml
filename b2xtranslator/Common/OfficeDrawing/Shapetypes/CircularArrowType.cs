using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(99)]
internal class CircularArrowType : ShapeType
{
    public CircularArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "al10800,10800@8@8@4@6,10800,10800,10800,10800@9@7l@30@31@17@18@24@25@15@16@32@33xe";
        Formulas = new List<string>
        {
            "val #1",
            "val #0",
            "sum #1 0 #0",
            "val 10800",
            "sum 0 0 #1",
            "sumangle @2 360 0",
            "if @2 @2 @5",
            "sum 0 0 @6",
            "val #2",
            "sum 0 0 #0",
            "sum #2 0 2700",
            "cos @10 #1 ",
            "sin @10 #1 ",
            "cos 13500 #1",
            "sin 13500 #1 ",
            "sum @11 10800 0",
            "sum @12 10800 0 ",
            "sum @13 10800 0 ",
            "sum @14 10800 0 ",
            "prod #2 1 2 ",
            "sum @19 5400 0",
            "cos @20 #1",
            "sin @20 #1",
            "sum @21 10800 0 ",
            "sum @12 @23 @22",
            "sum @22 @23 @11",
            "cos 10800 #1",
            "sin 10800 #1",
            "cos #2 #1 ",
            "sin #2 #1 ",
            "sum @26 10800 0",
            "sum @27 10800 0",
            "sum @28 10800 0",
            "sum @29 10800 0",
            "sum @19 5400 0 ",
            "cos @34 #0 ",
            "sin @34 #0 ",
            "mid #0 #1 ",
            "sumangle @37 180 0 ",
            "if @2 @37 @38",
            "cos 10800 @39 ",
            "sin 10800 @39 ",
            "cos #2 @39 ",
            "sin #2 @39 ",
            "sum @40 10800 0",
            "sum @41 10800 0 ",
            "sum @42 10800 0 ",
            "sum @43 10800 0 ",
            "sum @35 10800 0 ",
            "sum @36 10800 0"
        };
        
        AdjustmentValues = "-11796480,,5400";
        ConnectorLocations = "@44,@45;@48,@49;@46,@47;@17,@18;@24,@25;@15,@16";
        
        TextboxRectangle = "3163,3163,18437,18437";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "@3,#0",
            polar = "10800,10800"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "#2,#1",
            polar = "10800,10800",
            radiusrange = "0,10800"
        };
        Handles.Add(HandleTwo);
    }
}