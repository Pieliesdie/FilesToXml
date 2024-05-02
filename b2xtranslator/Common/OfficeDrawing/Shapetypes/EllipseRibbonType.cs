using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(107)]
public class EllipseRibbonType : ShapeType
{
    public EllipseRibbonType()
    {
        ShapeConcentricFill = false;
        
        Joins = JoinStyle.miter;
        
        Path =
            "ar@9@38@8@37,0@27@0@26@9@13@8@4@0@25@22@25@9@38@8@37@22@26@3@27l@7@40@3,wa@9@35@8@10@3,0@21@33@9@36@8@1@21@31@20@31@9@35@8@10@20@33,,l@5@40xewr@9@36@8@1@20@31@0@32nfl@20@33ear@9@36@8@1@21@31@22@32nfl@21@33em@0@26nfl@0@32em@22@26nfl@22@32e";
        
        Formulas = new List<string>
        {
            "val #0",
            "val #1",
            "val #2 ",
            "val width ",
            "val height ",
            "prod width 1 8",
            "prod width 1 2 ",
            "prod width 7 8 ",
            "prod width 3 2 ",
            "sum 0 0 @6 ",
            "sum height 0 #2",
            "prod @10 30573 4096",
            "prod @11 2 1 ",
            "sum height 0 @12",
            "sum @11 #2 0 ",
            "sum @11 height #1",
            "sum height 0 #1 ",
            "prod @16 1 2 ",
            "sum @11 @17 0 ",
            "sum @14 #1 height",
            "sum #0 @5 0 ",
            "sum width 0 @20",
            "sum width 0 #0",
            "sum @6 0 #0",
            "ellipse @23 width @11 ",
            "sum @24 height @11 ",
            "sum @25 @11 @19 ",
            "sum #2 @11 @19 ",
            "prod @11 2391 32768 ",
            "sum @6 0 @20 ",
            "ellipse @29 width @11 ",
            "sum #1 @30 @11 ",
            "sum @25 #1 height ",
            "sum height @30 @14 ",
            "sum @11 @14 0 ",
            "sum height 0 @34 ",
            "sum @35 @19 @11 ",
            "sum @10 @15 @11 ",
            "sum @35 @15 @11 ",
            "sum @28 @14 @18 ",
            "sum height 0 @39 ",
            "sum @19 0 @18 ",
            "prod @41 2 3 ",
            "sum #1 0 @42 ",
            "sum #2 0 @42 ",
            "min @44 20925 ",
            "prod width 3 8 ",
            "sum @46 0 4"
        };
        
        AdjustmentValues = "5400,5400,18900";
        
        ConnectorLocations = "@6,@1;@5,@40;@6,@4;@7,@40";
        
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@0,@1,@22,@25";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,bottomRight",
            xrange = "@5,@47"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle
        {
            position = "center,#1",
            yrange = "@10,@43"
        };
        Handles.Add(HandleTwo);
        
        var HandleThree = new Handle
        {
            position = "topLeft,#2",
            yrange = "@27,@45"
        };
        Handles.Add(HandleThree);
    }
}