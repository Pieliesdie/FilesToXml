using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(21)]
    class PlaqueType : ShapeType
    {
        public PlaqueType()
        {
            this.ShapeConcentricFill = true;
            this.Joins = JoinStyle.miter;
            this.Path = "m@0,qy0@0l0@2qx@0,21600l@1,21600qy21600@2l21600@0qx@1,xe";
            this.Formulas = new List<string>
            {
                "val #0 ",
                "sum width 0 #0 ",
                "sum height 0 #0 ",
                "prod @0 7071 10000 ",
                "sum width 0 @3 ",
                "sum height 0 @3 ",
                "val width ",
                "val height ",
                "prod width 1 2 ",
                "prod height 1 2"
            };

            this.AdjustmentValues = "3600";
            this.ConnectorLocations = "@8,0;0,@9;@8,@7;@6,@9";

            this.TextboxRectangle = "@3,@3,@4,@5";
            this.Handles = new List<Handle>();
            var HandleOne = new Handle
            {
                position = "#0,topLeft",
                switchHandle = "true",
                xrange = "0,10800"
            };
            this.Handles.Add(HandleOne);

            this.Limo = "10800,10800";
        }
    }
}
