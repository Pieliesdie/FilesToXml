using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(163)]
public class TextDeflateBottom : ShapeType
{
    public TextDeflateBottom()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "11475";
        Path = "m,l21600,m,21600c7200@1,14400@1,21600,21600e";
        ConnectorLocations = "10800,0;0,10800;10800,@2;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        Formulas = new List<string>
        {
            "prod #0 4 3",
            "sum @0 0 7200",
            "val #0",
            "prod #0 2 3",
            "sum @3 7200 0"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "center,#0",
            yrange = "1350,21600"
        };
        Handles.Add(h1);
    }
}