using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(54)]
public class RibbonUpType : ShapeType
{
    public RibbonUpType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        
        Path = "m0@29l@3@29qx@4@19l@4@10@5@10@5@19qy@6@29l@28@29@26@22@28@23@9@23@9@24qy@8,l@1,qx@0@24l@0@23,0@23,2700@22xem@4@19nfqy@3@20l@1@20qx@0@21@1@10l@4@10em@5@19nfqy@6@20l@8@20qx@9@21@8@10l@5@10em@0@21nfl@0@23em@9@21nfl@9@23e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum @0 675 0",
            "sum @1 675 0",
            "sum @2 675 0",
            "sum @3 675 0",
            "sum width 0 @4",
            "sum width 0 @3",
            "sum width 0 @2",
            "sum width 0 @1",
            "sum width 0 @0",
            "val #1",
            "prod @10 1 4",
            "prod @10 1 2",
            "prod @10 3 4",
            "prod height 3 4",
            "prod height 1 2",
            "prod height 1 4",
            "prod height 3 2",
            "prod height 2 3",
            "sum @11 @14 0",
            "sum @12 @15 0",
            "sum @13 @16 0",
            "sum @17 0 @20",
            "sum height 0 @10",
            "sum height 0 @19",
            "prod width 1 2",
            "sum width 0 2700",
            "sum @25 0 2700",
            "val width",
            "val height"
        };
        
        AdjustmentValues = "5400,18900";
        
        ConnectorLocations = "@25,0;2700,@22;@25,@10;@26,@22";
        
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "@0,0,@9,@10";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle();
        var HandleTwo = new Handle();
        HandleOne.position = "#0,topLeft";
        HandleOne.xrange = "2700,8100";
        HandleTwo.position = "center,#1";
        HandleTwo.yrange = "14400,21600";
        Handles.Add(HandleOne);
        Handles.Add(HandleTwo);
    }
}