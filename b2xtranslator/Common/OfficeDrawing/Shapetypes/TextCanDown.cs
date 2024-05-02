using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(175)]
public class TextCanDown : ShapeType
{
    public TextCanDown()
    {
        TextPath = true;
        
        Path = "m,qy10800@0,21600,m0@1qy10800,21600,21600@1e";
        
        Formulas = new List<string>
        {
            "val #0",
            "sum 21600 0 #0",
            "prod @1 1 2",
            "sum @2 10800 0"
        };
        
        ConnectorLocations = "10800,@0;0,@2;10800,21600;21600,@2";
        ConnectorAngles = "270,180,90,0";
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "center,#0",
            yrange = "0,7200"
        };
        Handles.Add(h1);
    }
}