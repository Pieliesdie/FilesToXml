using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(163)]
    public class TextDeflateBottom : ShapeType
    {
        public TextDeflateBottom()
        {
            this.TextPath = true;
            this.Joins = JoinStyle.none;
            this.AdjustmentValues = "11475";
            this.Path = "m,l21600,m,21600c7200@1,14400@1,21600,21600e";
            this.ConnectorLocations = "10800,0;0,10800;10800,@2;21600,10800";
            this.ConnectorAngles = "270,180,90,0";

            this.Formulas = new List<string>
            {
                "prod #0 4 3",
                "sum @0 0 7200",
                "val #0",
                "prod #0 2 3",
                "sum @3 7200 0"
            };

            this.Handles = new List<Handle>();
            var h1 = new Handle
            {
                position = "center,#0",
                yrange = "1350,21600"
            };
            this.Handles.Add(h1);

        }
    }
}
