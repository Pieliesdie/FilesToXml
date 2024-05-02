using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(161)]
public class TextDeflate : ShapeType
{
    public TextDeflate()
    {
        TextPath = true;
        
        Path = "m,c7200@0,14400@0,21600,m,21600c7200@1,14400@1,21600,21600e";
        
        Formulas = new List<string>
        {
            "prod #0 4 3",
            "sum 21600 0 @0",
            "val #0",
            "sum 21600 0 #0"
        };
        
        ConnectorLocations = "10800,@2;0,10800;10800,@3;21600,10800";
        ConnectorAngles = "270,180,90,0";
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "center,#0",
            yrange = "0,8100"
        };
        Handles.Add(h1);
    }
}