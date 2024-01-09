using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(75)]
    public class PictureFrameType : ShapeType
    {
        public PictureFrameType()
        {
            this.Path = "m@4@5l@4@11@9@11@9@5xe";

            this.Formulas = new List<string>
            {
                "if lineDrawn pixelLineWidth 0",
                "sum @0 1 0",
                "sum 0 0 @1",
                "prod @2 1 2",
                "prod @3 21600 pixelWidth",
                "prod @3 21600 pixelHeight",
                "sum @0 0 1",
                "prod @6 1 2",
                "prod @7 21600 pixelWidth",
                "sum @8 21600 0",
                "prod @7 21600 pixelHeight",
                "sum @10 21600 0"
            };

            //pictures are not stroked or fileld by default
            this.Filled = false;
            this.Stroked = false;

            //pictures have a lock on the aspect ratio by default
            this.Lock = new ProtectionBooleans(0)
            {
                fUsefLockAspectRatio = true,
                fLockAspectRatio = true
            };

            this.ShapeConcentricFill = true;
            this.ConnectorType = "rect";
        }   
    }       
}           
            

