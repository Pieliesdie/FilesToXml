using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(137)]
public class TextStop : ShapeType
{
    public TextStop()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        ExtrusionOk = true;
        Lock = new ProtectionBooleans
        {
            fUsefLockText = true,
            fLockText = true
        };
        LockShapeType = true;
        
        AdjustmentValues = "4800";
        Path = "m0@0l7200,r7200,l21600@0m0@1l7200,21600r7200,l21600@1e";
        ConnectorType = "rect";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 @0"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "topLeft,#0",
            yrange = "3086,10800"
        };
        Handles.Add(h1);
    }
}