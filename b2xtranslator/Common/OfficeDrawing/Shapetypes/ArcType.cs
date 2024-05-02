using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(19)]
internal class ArcType : ShapeType
{
    public ArcType()
    {
        ShapeConcentricFill = true;
        Joins = JoinStyle.round;
        Path = "wr-21600,,21600,43200,,,21600,21600nfewr-21600,,21600,43200,,,21600,21600l,21600nsxe";
        Formulas = new List<string>
        {
            "val #2",
            "val #3",
            "val #4"
        };
        
        AdjustmentValues = "-5898240,,,21600,21600";
        ConnectorLocations = "0,0;21600,21600;0,21600";
        
        Handles = new List<Handle>();
        var HandleOne = new Handle
        {
            position = "@2,#0",
            polar = "@0,@1"
        };
        Handles.Add(HandleOne);
        
        var HandleTwo = new Handle();
        HandleOne.position = "@2,#1";
        HandleOne.polar = "@0,@1";
        Handles.Add(HandleTwo);
    }
}