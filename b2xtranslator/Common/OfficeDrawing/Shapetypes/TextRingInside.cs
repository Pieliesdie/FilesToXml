using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(142)]
public class TextRingInside : ShapeType
{
    public TextRingInside()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "13500";
        Path = "m0@1qy10800,,21600@1,10800@0,0@1m0@2qy10800@3,21600@2,10800,21600,0@2e";
        ConnectorType = "custom";
        ConnectorLocations = "10800,0;10800,@0;0,10800;10800,21600;10800,@3;21600,10800";
        ConnectorAngles = "270,270,180,90,90,0";
        ExtrusionOk = true;
        Lock = new ProtectionBooleans
        {
            fUsefLockText = true,
            fLockText = true
        };
        LockShapeType = true;
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 1 2",
            "sum height 0 @1",
            "sum height 0 #0",
            "sum @2 0 @1"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "center,#0",
            yrange = "10800,21600"
        };
        Handles.Add(h1);
    }
}