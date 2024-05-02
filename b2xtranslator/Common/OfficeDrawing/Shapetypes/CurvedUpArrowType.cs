using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(104)]
internal class CurvedUpArrowType : ShapeType
{
    public CurvedUpArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "ar0@22@3@21,,0@4@21@14@22@1@21@7@21@12@2l@13@2@8,0@11@2wa0@22@3@21@10@2@16@24@14@22@1@21@16@24@14,xewr@14@22@1@21@7@21@16@24nfe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2",
            "sum #0 width #1",
            "prod @3 1 2",
            "sum #1 #1 width ",
            "sum @5 #1 #0",
            "prod @6 1 2",
            "mid width #0",
            "ellipse #2 height @4",
            "sum @4 @9 0 ",
            "sum @10 #1 width",
            "sum @7 @9 0 ",
            "sum @11 width #0 ",
            "sum @5 0 #0 ",
            "prod @14 1 2 ",
            "mid @4 @7 ",
            "sum #0 #1 width ",
            "prod @17 1 2 ",
            "sum @16 0 @18 ",
            "val width ",
            "val height ",
            "sum 0 0 height",
            "sum @16 0 @4 ",
            "ellipse @23 @4 height ",
            "sum @8 128 0 ",
            "prod @5 1 2 ",
            "sum @5 0 128 ",
            "sum #0 @16 @11 ",
            "sum width 0 #0 ",
            "prod @29 1 2 ",
            "prod height height 1 ",
            "prod #2 #2 1 ",
            "sum @31 0 @32 ",
            "sqrt @33 ",
            "sum @34 height 0 ",
            "prod width height @35",
            "sum @36 64 0 ",
            "prod #0 1 2 ",
            "ellipse @30 @38 height ",
            "sum @39 0 64 ",
            "prod @4 1 2",
            "sum #1 0 @41 ",
            "prod height 4390 32768",
            "prod height 28378 32768"
        };
        
        AdjustmentValues = "12960,19440,7200";
        ConnectorLocations = "@8,0;@11,@2;@15,0;@16,@21;@13,@2";
        ConnectorAngles = "270,270,270,90,0";
        
        TextboxRectangle = "@41,@43,@42,@44";
        
        Handles = new List<Handle>();
        
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "@37,@27"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle();
        HandleOne.position = "#1,topLeft";
        HandleOne.xrange = "@25,@20";
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "bottomRight,#2",
            yrange = "0,@40"
        };
        Handles.Add(HandleThree);
    }
}