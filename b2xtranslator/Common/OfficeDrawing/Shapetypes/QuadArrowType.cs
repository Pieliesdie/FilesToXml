using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(76)]
internal class QuadArrowType : ShapeType
{
    public QuadArrowType()
    {
        ShapeConcentricFill = false;
        Joins = JoinStyle.miter;
        Path = "m10800,l@0@2@1@2@1@1@2@1@2@0,,10800@2@3@2@4@1@4@1@5@0@5,10800,21600@3@5@4@5@4@4@5@4@5@3,21600,10800@5@0@5@1@4@1@4@2@3@2xe";
        Formulas = new List<string>
        {
            "val #0 ",
            "val #1",
            "val #2 ",
            "sum 21600 0 #0 ",
            "sum 21600 0 #1 ",
            "sum 21600 0 #2 ",
            "sum #0 0 10800 ",
            "sum #1 0 10800 ",
            "prod @7 #2 @6 ",
            "sum 21600 0 @8"
        };
        
        AdjustmentValues = "6480,8640,4320";
        ConnectorLocations = "Rectangle";
        
        TextboxRectangle = "@8,@1,@9,@4;@1,@8,@4,@9";
        
        Handles = new List<Handle>();
        
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "@2,@1"
        };
        Handles.Add(HandleOne);
        var HandleTwo = new Handle
        {
            position = "#1,#2",
            xrange = "@0,10800",
            yrange = "0,@0"
        };
        Handles.Add(HandleTwo);
    }
}