using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class LineStyleBooleans
{
    public bool fArrowheadsOK;
    public bool fHitTestLine;
    public bool fInsetPen;
    public bool fInsetPenOK;
    public bool fLine;
    public bool fLineFillShape;
    public bool fLineOpaqueBackColor;
    public bool fNoLineDrawDash;
    public bool fUsefArrowheadsOK;
    public bool fUsefHitTestLine;
    public bool fUsefInsetPen;
    public bool fUsefInsetPenOK;
    public bool fUsefLine;
    public bool fUsefLineFillShape;
    public bool fUsefLineOpaqueBackColor;
    public bool fUsefNoLineDrawDash;
    
    public LineStyleBooleans(uint entryOperand)
    {
        fNoLineDrawDash = Utils.BitmaskToBool(entryOperand, 0x1);
        fLineFillShape = Utils.BitmaskToBool(entryOperand, 0x2);
        fHitTestLine = Utils.BitmaskToBool(entryOperand, 0x4);
        fLine = Utils.BitmaskToBool(entryOperand, 0x8);
        
        fArrowheadsOK = Utils.BitmaskToBool(entryOperand, 0x10);
        fInsetPenOK = Utils.BitmaskToBool(entryOperand, 0x20);
        fInsetPen = Utils.BitmaskToBool(entryOperand, 0x40);
        
        //Reserved 0x80 0x100
        
        fLineOpaqueBackColor = Utils.BitmaskToBool(entryOperand, 0x200);
        
        //Unused 0x400 0x800 0x1000 0x2000 0x4000 0x8000
        
        fUsefNoLineDrawDash = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefLineFillShape = Utils.BitmaskToBool(entryOperand, 0x20000);
        fUsefHitTestLine = Utils.BitmaskToBool(entryOperand, 0x40000);
        fUsefLine = Utils.BitmaskToBool(entryOperand, 0x80000);
        fUsefArrowheadsOK = Utils.BitmaskToBool(entryOperand, 0x100000);
        fUsefInsetPenOK = Utils.BitmaskToBool(entryOperand, 0x200000);
        fUsefInsetPen = Utils.BitmaskToBool(entryOperand, 0x400000);
        
        //Reserved 0x800000 0x1000000
        
        fUsefLineOpaqueBackColor = Utils.BitmaskToBool(entryOperand, 0x2000000);
    }
}