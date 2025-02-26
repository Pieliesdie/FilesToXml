﻿using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes;

[OfficeShapeType(150)]
public class TextCirclePour : ShapeType
{
    public TextCirclePour()
    {
        TextPath = true;
        Joins = JoinStyle.none;
        AdjustmentValues = "-11730944,5400";
        Path = "al10800,10800,10800,10800@2@5al10800,10800@0@0@2@5e";
        ConnectorLocations = "@17,10800;@12,@13;@16,10800;@12,@14";
        ExtrusionOk = true;
        Lock = new ProtectionBooleans
        {
            fUsefLockText = true,
            fLockText = true
        };
        LockShapeType = true;
        
        Formulas = new List<string>
        {
            "val #1",
            "val #0",
            "sum 0 0 #0",
            "prod #0 2 1",
            "sumangle @3 0 360",
            "if @3 @4 @3",
            "val 10800",
            "sum 10800 0 #1",
            "prod #1 1 2",
            "sum @8 5400 0",
            "cos @9 #0",
            "sin @9 #0",
            "sum @10 10800 0",
            "sum @11 10800 0",
            "sum 10800 0 @11",
            "sum #1 10800 0",
            "if #0 @7 @15",
            "if #0 0 21600"
        };
        
        Handles = new List<Handle>();
        var h1 = new Handle
        {
            position = "#1,#0",
            polar = "10800,10800",
            radiusrange = "0,10800"
        };
        Handles.Add(h1);
    }
}