using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{

    [OfficeShapeType(184)]
    class MoonType : ShapeType
    {
        public MoonType()
        {
            this.ShapeConcentricFill = false;
            this.Joins = JoinStyle.miter;
            this.Path = "m21600,qx,10800,21600,21600wa@0@10@6@11,21600,21600,21600,xe";
            this.Formulas = new List<string>
            {
                "val #0 ",
                "sum 21600 0 #0 ",
                "prod #0 #0 @1 ",
                "prod 21600 21600 @1 ",
                "prod @3 2 1 ",
                "sum @4 0 @2",
                "sum @5 0 #0 ",
                "prod @5 1 2 ",
                "sum @7 0 #0 ",
                "prod @8 1 2 ",
                "sum 10800 0 @9 ",
                "sum @9 10800 0 ",
                "prod #0 9598 32768 ",
                " sum 21600 0 @12 ",
                "ellipse @13 21600 10800 ",
                "sum 10800 0 @14 ",
                "sum @14 10800 0"
            };

            this.AdjustmentValues = "10800";
            this.ConnectorAngles = "270,180,90,0"; 
            this.ConnectorLocations = "21600,0;0,10800;21600,21600;@0,10800";

            this.TextboxRectangle = "@12,@15,@0,@16";
            this.Handles = new List<Handle>();
            var HandleOne = new Handle
            {
                position = "#0,center",
                xrange = "0,18900"
            };
            this.Handles.Add(HandleOne);

        }
    }
}
