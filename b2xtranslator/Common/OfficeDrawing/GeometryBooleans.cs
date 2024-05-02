using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class GeometryBooleans
{
    public bool f3DOK;
    public bool fFillOK;
    public bool fFillShadeShapeOK;
    public bool fGtextOK;
    public bool fLineOK;
    public bool fShadowOK;
    public bool fUsef3DOK;
    public bool fUsefFillOK;
    public bool fUsefFillShadeShapeOK;
    public bool fUsefGtextOK;
    public bool fUsefLineOK;
    public bool fUsefShadowOK;
    
    public GeometryBooleans(uint entryOperand)
    {
        fFillOK = Utils.BitmaskToBool(entryOperand, 0x1);
        fFillShadeShapeOK = Utils.BitmaskToBool(entryOperand, 0x2);
        fGtextOK = Utils.BitmaskToBool(entryOperand, 0x4);
        fLineOK = Utils.BitmaskToBool(entryOperand, 0x8);
        f3DOK = Utils.BitmaskToBool(entryOperand, 0x10);
        fShadowOK = Utils.BitmaskToBool(entryOperand, 0x20);
        
        fUsefFillOK = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefFillShadeShapeOK = Utils.BitmaskToBool(entryOperand, 0x20000);
        fUsefGtextOK = Utils.BitmaskToBool(entryOperand, 0x40000);
        fUsefLineOK = Utils.BitmaskToBool(entryOperand, 0x80000);
        fUsef3DOK = Utils.BitmaskToBool(entryOperand, 0x100000);
        fUsefShadowOK = Utils.BitmaskToBool(entryOperand, 0x200000);
    }
}