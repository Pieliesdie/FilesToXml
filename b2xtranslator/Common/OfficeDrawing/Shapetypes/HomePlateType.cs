using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(15)]
internal class HomePlateType : ShapeType
{
    public HomePlateType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m@0,l,,,21600@0,21600,21600,10800xe";
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 1 2"
        };
        
        AdjustmentValues = "16200";
        ConnectorLocations = "@1,0;0,10800;@1,21600;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        TextboxRectangle = "0,0,10800,21600;0,0,16200,21600;0,0,21600,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,topLeft",
            xrange = "0,21600"
        };
        
        Handles.Add(HandleOne);
    }
}