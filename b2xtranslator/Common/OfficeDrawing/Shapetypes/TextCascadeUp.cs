using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(154)]
    public class TextCascadeUp : ShapeType
    {
        public TextCascadeUp()
        {
            this.TextPath = true;

            this.Path = "m0@2l21600,m,21600l21600@0e";

            this.ConnectorLocations = "10800,@4;0,@6;10800,@5;21600,@3";
            this.ConnectorAngles = "270,180,90,0";

            this.Formulas = new List<string>
            {
                "val #0",
                "sum 21600 0 #0",
                "prod @1 1 4",
                "prod #0 1 2",
                "prod @2 1 2",
                "sum @3 10800 0",
                "sum @4 10800 0",
                "sum @0 21600 @2",
                "prod @7 1 2"
            };

            this.Handles = new List<Handle>();
            var h1 = new Handle
            {
                position = "bottomRight,#0",
                yrange = "6171,21600"
            };
            this.Handles.Add(h1);

        }

    }
}
