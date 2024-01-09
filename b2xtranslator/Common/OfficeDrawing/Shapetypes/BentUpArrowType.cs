using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(90)]
    class BentUpArrowType : ShapeType
    {
        public BentUpArrowType()
        {
            this.ShapeConcentricFill = false;
            this.Joins = JoinStyle.miter;
            this.Path = "m@4,l@0@2@5@2@5@12,0@12,,21600@1,21600@1@2,21600@2xe";
            this.Formulas = new List<string>
            {
                "val #0",
                "val #1",
                "val #2",
                "prod #0 1 2",
                "sum @3 10800 0",
                "sum 21600 #0 #1",
                "sum #1 #2 0",
                "prod @6 1 2",
                "prod #1 2 1",
                "sum @8 0 21600",
                "prod 21600 @0 @1",
                "prod 21600 @4 @1",
                "prod 21600 @5 @1",
                "prod 21600 @7 @1",
                "prod #1 1 2",
                "sum @5 0 @4",
                "sum @0 0 @4",
                "prod @2 @15 @16"
            };

            this.AdjustmentValues = "9257,18514,7200";
            this.ConnectorLocations = "@4,0;@0,@2;0,@11;@14,21600;@1,@13;21600,@2";
            this.ConnectorAngles = "270,180,180,90,0,0";

            this.TextboxRectangle = "0,@12,@1,21600;@5,@17,@1,21600";
           
            this.Handles = new List<Handle>();



            var HandleOne = new Handle
            {
                position = "#0,topLeft",
                xrange = "@2,@9"
            };
            this.Handles.Add(HandleOne);

            var HandleTwo = new Handle
            {
                position = "#1,#2",
                xrange = "@4,21600",
                yrange = "0,@0"
            };
            this.Handles.Add(HandleTwo);
        }
    }
}
