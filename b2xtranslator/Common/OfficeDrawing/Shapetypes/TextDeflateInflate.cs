using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(166)]
public class TextDeflateInflate : ShapeType
{
    public TextDeflateInflate()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "6054";
        Path = "m,l21600,m,10125c7200@1,14400@1,21600,10125m,11475c7200@2,14400@2,21600,11475m,21600r21600,e";
        ConnectorType = "rect";
        
        ExtrusionOk = true;
        Lock = new ProtectionBooleans
        {
            fUsefLockText = true,
            fLockText = true
        };
        LockShapeType = true;
        
        Formulas = new List<string>
        {
            "prod #0 4 3",
            "sum @0 0 4275",
            "sum @0 0 2925"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "center,#0",
            yrange = "1308,20292"
        };
        Handles.Add(h1);
    }
}