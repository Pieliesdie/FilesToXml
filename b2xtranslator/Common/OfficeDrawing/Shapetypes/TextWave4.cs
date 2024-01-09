using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(159)]
    public class TextWave4 : ShapeType
    {
        public TextWave4()
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

            this.AdjustmentValues = "1404,10800";
            this.Path = "m@37@0c@38@1@39@3@40@0@41@1@42@3@43@0m@30@4c@31@6@32@5@33@4@34@6@35@5@36@4e";
            this.ConnectorLocations = "@40,@0;@51,10800;@33,@4;@50,10800";
            this.ConnectorAngles = "270,180,90,0";

            this.Formulas = new List<string>
            {
                "val #0",
                "prod @0 41 9",
                "prod @0 23 9",
                "sum 0 0 @2",
                "sum 21600 0 #0",
                "sum 21600 0 @1",
                "sum 21600 0 @3",
                "sum #1 0 10800",
                "sum 21600 0 #1",
                "prod @8 1 3",
                "prod @8 2 3",
                "prod @8 4 3",
                "prod @8 5 3",
                "prod @8 2 1",
                "sum 21600 0 @9",
                "sum 21600 0 @10",
                "sum 21600 0 @8",
                "sum 21600 0 @11",
                "sum 21600 0 @12",
                "sum 21600 0 @13",
                "prod #1 1 3",
                "prod #1 2 3",
                "prod #1 4 3",
                "prod #1 5 3",
                "prod #1 2 1",
                "sum 21600 0 @20",
                "sum 21600 0 @21",
                "sum 21600 0 @22",
                "sum 21600 0 @23",
                "sum 21600 0 @24",
                "if @7 @19 0",
                "if @7 @18 @20",
                "if @7 @17 @21",
                "if @7 @16 #1",
                "if @7 @15 @22",
                "if @7 @14 @23",
                "if @7 21600 @24",
                "if @7 0 @29",
                "if @7 @9 @28",
                "if @7 @10 @27",
                "if @7 @8 @8",
                "if @7 @11 @26",
                "if @7 @12 @25",
                "if @7 @13 21600",
                "sum @36 0 @30",
                "sum @4 0 @0",
                "max @30 @37",
                "min @36 @43",
                "prod @0 2 1",
                "sum 21600 0 @48",
                "mid @36 @43",
                "mid @30 @37"
            };

            this.Handles = new List<Handle>();
            var h1 = new Handle
            {
                position = "topLeft,#0",
                yrange = "0,2229"
            };
            this.Handles.Add(h1);
            var h2 = new Handle
            {
                position = "#1,bottomRight",
                xrange = "8640,12960"
            };
            this.Handles.Add(h2);
        }
    }
}
