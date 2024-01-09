using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(148)]
    public class TextArchUpPour : ShapeType
    {
        public TextArchUpPour()
        {
            this.TextPath = true;
            this.Joins = JoinStyle.none;
            this.ExtrusionOk = true;
            this.Lock = new ProtectionBooleans
            {
                fUsefLockText = true,
                fLockText = true
            };
            this.LockShapeType = true;

            this.AdjustmentValues = "11796480,5400";
            this.Path = "al10800,10800,10800,10800@2@14al10800,10800@0@0@2@14e";
            this.ConnectorLocations = "10800,@27;@22,@23;10800,@26;@24,@23";

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
                "sum 10800 0 #1",
                "prod #1 1 2",
                "sum @18 5400 0",
                "cos @19 #0",
                "sin @19 #0",
                "sum @20 10800 0",
                "sum @21 10800 0",
                "sum 10800 0 @20",
                "sum #1 10800 0",
                "if @9 @17 @25",
                "if @9 0 21600"
            };

            this.Handles = new List<Handle>();
            var h1 = new Handle
            {
                position = "#1,#0",
                polar = "10800,10800",
                radiusrange = "0,10800"
            };
            this.Handles.Add(h1);
        }
    }
}
