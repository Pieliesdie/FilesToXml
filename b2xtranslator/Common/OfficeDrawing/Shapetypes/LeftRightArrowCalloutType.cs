using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(81)]
    class LeftRightArrowCalloutType : ShapeType
    {
        public LeftRightArrowCalloutType()
        {
            this.ShapeConcentricFill = false;
            this.Joins = JoinStyle.miter;
            this.Path = "m@0,l@0@3@2@3@2@1,,10800@2@4@2@5@0@5@0,21600@8,21600@8@5@9@5@9@4,21600,10800@9@1@9@3@8@3@8,xe"; 
            this.Formulas = new List<string>
            {
                "val #0",
                "val #1",
                "val #2",
                "val #3",
                "sum 21600 0 #1",
                "sum 21600 0 #3",
                "sum #0 21600 0",
                "prod @6 1 2",
                "sum 21600 0 #0",
                "sum 21600 0 #2"
            };

            this.AdjustmentValues = "5400,5400,2700,8100";
            this.ConnectorLocations = "10800,0;0,10800;10800,21600;21600,10800";
            this.ConnectorAngles = "270,180,90,0"; 

            this.TextboxRectangle = "@0,0,@8,21600";
            
            this.Handles = new List<Handle>();
            var HandleOne = new Handle
            {
                position = "#0,topLeft",
                xrange = "@2,10800"
            };
            this.Handles.Add(HandleOne);

            var HandleTwo = new Handle
            {
                position = "topLeft,#1",
                yrange = "0,@3"
            };
            this.Handles.Add(HandleTwo);

            var HandleThree = new Handle
            {
                position = "#2,#3",
                xrange = "0,@0",
                yrange = "@1,10800"
            };
            this.Handles.Add(HandleThree); 


        }
    }
}


