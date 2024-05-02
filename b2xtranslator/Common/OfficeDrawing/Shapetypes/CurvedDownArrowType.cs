using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(105)]
internal class CurvedDownArrowType : ShapeType
{
    public CurvedDownArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "wr,0@3@23,0@22@4,0@15,0@1@23@7,0@13@2l@14@2@8@22@12@2at,0@3@23@11@2@17@26@15,0@1@23@17@26@15@22xewr,0@3@23@4,0@17@26nfe";
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2 ",
            "sum #0 width #1 ",
            "prod @3 1 2 ",
            "sum #1 #1 width",
            "sum @5 #1 #0 ",
            "prod @6 1 2",
            "mid width #0 ",
            "sum height 0 #2 ",
            "ellipse @9 height @4",
            "sum @4 @10 0",
            "sum @11 #1 width",
            "sum @7 @10 0",
            "sum @12 width #0 ",
            "sum @5 0 #0 ",
            "prod @15 1 2",
            "mid @4 @7 ",
            "sum #0 #1 width",
            "prod @18 1 2 ",
            "sum @17 0 @19 ",
            "val width ",
            "val height ",
            "prod height 2 1",
            "sum @17 0 @4 ",
            "ellipse @24 @4 height",
            "sum height 0 @25",
            "sum @8 128 0",
            "prod @5 1 2",
            "sum @5 0 128",
            "sum #0 @17 @12",
            "ellipse @20 @4 height",
            "sum width 0 #0",
            "prod @32 1 2",
            "prod height height 1",
            "prod @9 @9 1",
            "sum @34 0 @35",
            "sqrt @36",
            "sum @37 height 0",
            "prod width height @38",
            "sum @39 64 0",
            "prod #0 1 2",
            "ellipse @33 @41 height",
            "sum height 0 @42",
            "sum @43 64 0",
            "prod @4 1 2",
            "sum #1 0 @45",
            "prod height 4390 32768",
            "prod height 28378 32768"
        };
        
        AdjustmentValues = "12960,19440,14400";
        ConnectorLocations = "@17,0;@16,@22;@12,@2;@8,@22;@14,@2";
        ConnectorAngles = "270,90,90,90,0";
        
        TextboxRectangle = "@45,@47,@46,@48";
        
        Handles = new List<Handle>();
        
        var HandleOne = new Handle
        {
            position = "#0,bottomRight",
            xrange = "@40,@29"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle();
        HandleOne.position = "#1,bottomRight";
        HandleOne.xrange = "@27,@21";
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "bottomRight,#2",
            yrange = "@44,@22"
        };
        Handles.Add(HandleThree);
    }
}