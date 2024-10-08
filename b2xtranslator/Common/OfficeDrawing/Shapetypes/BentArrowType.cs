using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(91)]
internal class BentArrowType : ShapeType
{
    public BentArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m21600,6079l@0,0@0@1,12427@1qx,12158l,21600@4,21600@4,12158qy12427@2l@0@2@0,12158xe";
        Formulas = new List<string>
        {
            "val #0 ",
            "val #1 ",
            "sum 12158 0 #1 ",
            "sum @2 0 #1 ",
            "prod @3 32768 32059 ",
            "prod @4 1 2 ",
            "sum 21600 0 #0 ",
            "prod @6 #1 6079 ",
            "sum @7 #0 0"
        };
        
        AdjustmentValues = "Connector Angles";
        ConnectorLocations = "@0,0;@0,12158;@5,21600;21600,6079";
        ConnectorAngles = "270,90,90,0";
        
        TextboxRectangle = "12427,@1,@8,@2;0,12158,@4,21600";
        
        Handles = new List<Handle>();
        
        var HandleOne = new Handle
        {
            position = "#0,#1",
            xrange = "12427,21600",
            yrange = "0,6079"
        };
        Handles.Add(HandleOne);
    }
}