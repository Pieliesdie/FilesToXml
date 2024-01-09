using System.Collections.Generic;

namespace b2xtranslator.OfficeDrawing.Shapetypes
{
    [OfficeShapeType(102)]
    class CurvedRightArrowType : ShapeType
    {
        public CurvedRightArrowType()
        {
            this.ShapeConcentricFill = false;
            this.Joins = JoinStyle.miter;
            this.Path = "ar,0@23@3@22,,0@4,0@15@23@1,0@7@2@13l@2@14@22@8@2@12wa,0@23@3@2@11@26@17,0@15@23@1@26@17@22@15xear,0@23@3,0@4@26@17nfe";
            this.Formulas = new List<string>
            {
                "val #0",
                "val #1",
                "val #2",
                "sum #0 width #1",
                "prod @3 1 2",
                "sum #1 #1 width",
                "sum @5 #1 #0",
                "prod @6 1 2",
                "mid width #0",
                "sum height 0 #2",
                "ellipse @9 height @4",
                "sum @4 @10 0",
                "sum @11 #1 width",
                "sum @7 @10 0",
                "sum @12 width #0",
                "sum @5 0 #0",
                "prod @15 1 2",
                "mid @4 @7",
                "sum #0 #1 width",
                "prod @18 1 2",
                "sum @17 0 @19",
                "val width",
                "val height",
                "prod height 2 1",
                "sum @17 0 @4",
                "ellipse @24 @4 height",
                "sum height 0 @25",
                "sum @8 128 0",
                "prod @5 1 2",
                "sum @5 0 128",
                "sum #0 @17 @12",
                "ellipse @20 @4 height",
                "sum width 0 #0",
                "prod @32 1 2",
                "prod height height 1",
                "prod @9 @9 1",
                "sum @34 0 @35",
                "sqrt @36",
                "sum @37 height 0",
                "prod width height @38",
                "sum @39 64 0",
                "prod #0 1 2",
                "ellipse @33 @41 height",
                "sum height 0 @42",
                "sum @43 64 0",
                "prod @4 1 2",
                "sum #1 0 @45",
                "prod height 4390 32768",
                "prod height 28378 32768"
            };

            this.AdjustmentValues = "12960,19440,14400";
            this.ConnectorLocations = "0,@17;@2,@14;@22,@8;@2,@12;@22,@16";
            this.ConnectorAngles = "180,90,0,0,0";

            this.TextboxRectangle = "@47,@45,@48,@46";
           
            this.Handles = new List<Handle>();

            var HandleOne = new Handle
            {
                position = "bottomRight,#0",
                yrange = "@40,@29"
            };
            this.Handles.Add(HandleOne);
            
            var HandleTwo = new Handle();
            HandleOne.position="bottomRight,#1"; 
            HandleOne.yrange="@27,@21";  
            this.Handles.Add(HandleTwo);

            var HandleThree = new Handle
            {
                position = "#2,bottomRight",
                xrange = "@44,@22"
            };
            this.Handles.Add(HandleThree);
        }
    }
}
