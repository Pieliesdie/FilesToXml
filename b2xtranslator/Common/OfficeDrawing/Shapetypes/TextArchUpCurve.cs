using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(144)]
    public class TextArchUpCurve : ShapeType
    {
        public TextArchUpCurve()
        {
            this.TextPath = true;
            this.Joins = JoinStyle.none;
            this.AdjustmentValues = "11796480";
            this.Path = "al10800,10800,10800,10800@2@14e";
            this.ConnectorLocations = "10800,@22;@19,@20;@21,@20";
            this.PreferRelative = false;
            this.TextKerning = true;
            this.ExtrusionOk = true;
            this.Lock = new ProtectionBooleans
            {
                fUsefLockText = true,
                fLockText = true
            };
            this.LockShapeType = true;

            this.Formulas = new List<string>
            {
                "val #1",
                "val #0",
                "sum 0 0 #0",
                "sumangle #0 0 180",
                "sumangle #0 0 90",
                "prod @4 2 1",
                "sumangle #0 90 0",
                "prod @6 2 1",
                "abs #0",
                "sumangle @8 0 90",
                "if @9 @7 @5",
                "sumangle @10 0 360",
                "if @10 @11 @10",
                "sumangle @12 0 360",
                "if @12 @13 @12",
                "sum 0 0 @14",
                "val 10800",
                "cos 10800 #0",
                "sin 10800 #0",
                "sum @17 10800 0",
                "sum @18 10800 0",
                "sum 10800 0 @17",
                "if @9 0 21600",
                "sum 10800 0 @18"
            };

            this.Handles = new List<Handle>();
            var h1 = new Handle
            {
                polar = "10800,10800",
                position = "@16,#0"
            };
            this.Handles.Add(h1);
        }
    }
}
