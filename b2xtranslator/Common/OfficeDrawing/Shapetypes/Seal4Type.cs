using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(187)]
internal class Seal4Type : ShapeType
{
    public Seal4Type()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.miter;
        Path = "m21600,10800l@2@3,10800,0@3@3,,10800@3@2,10800,21600@2@2xe";
        
        Formulas = new List<string>
        {
            "sum 10800 0 #0",
            "prod @0 23170 32768",
            "sum @1 10800 0",
            "sum 10800 0 @1"
        };
        
        AdjustmentValues = "8100";
        ConnectorLocations = "Rectangle";
        TextboxRectangle = "@3,@3,@2,@2";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "#0,center",
            xrange = "0,10800"
        };
        
        Handles.Add(HandleOne);
    }
}