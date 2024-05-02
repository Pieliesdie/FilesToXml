using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(153)]
public class TextCurveDown : ShapeType
{
    public TextCurveDown()
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
        
        AdjustmentValues = "9391";
        Path = "m,c7200@1,14400@2,21600@0m0@5c7200@6,14400@6,21600@5e";
        ConnectorLocations = "10800,@10;0,@8;10800,21600;21600,@9";
        ConnectorAngles = "270,180,90,0";
        
        Formulas = new List<string>
        {
            "val #0",
            "prod #0 3 4",
            "prod #0 5 4",
            "prod #0 3 8",
            "prod #0 1 8",
            "sum 21600 0 @3",
            "sum @4 21600 0",
            "prod #0 1 2",
            "prod @5 1 2",
            "sum @7 @8 0",
            "prod #0 7 8",
            "prod @5 1 3",
            "sum @1 @2 0",
            "sum @12 @0 0",
            "prod @13 1 4",
            "sum @11 14400 @14"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "bottomRight,#0",
            yrange = "0,11368"
        };
        Handles.Add(h1);
    }
}