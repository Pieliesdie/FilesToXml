using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(5)]
public class IsoscelesTriangleType : ShapeType
{
    public IsoscelesTriangleType()
    {
        ShapeConcentricFill = true;
        
        Joins = JoinStyle.miter;
        
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        
        Path = "m@0,l,21600r21600,xe";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 1 2",
            "sum @1 10800 0"
        };
        
        AdjustmentValues = "10800";
        
        ConnectorLocations = "@0,0;@1,10800;0,21600;10800,21600;21600,21600;@2,10800";
        
        TextboxRectangle = "0,10800,10800,18000;5400,10800,16200,18000;10800,10800,21600,18000;0,7200,7200,21600;7200,7200,14400,21600;14400,7200,21600,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,21600"
        };
        Handles.Add(HandleOne);
    }
}