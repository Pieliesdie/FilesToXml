using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class ProtectionBooleans
{
    public bool fLockAdjustHandles;
    public bool fLockAgainstGrouping;
    public bool fLockAgainstSelect;
    public bool fLockAgainstUngrouping;
    public bool fLockAspectRatio;
    public bool fLockCropping;
    public bool fLockPosition;
    public bool fLockRotation;
    public bool fLockText;
    public bool fLockVertices;
    public bool fUsefLockAdjustHandles;
    public bool fUsefLockAgainstGrouping;
    public bool fUsefLockAgainstSelect;
    public bool fUsefLockAgainstUngrouping;
    public bool fUsefLockAspectRatio;
    public bool fUsefLockCropping;
    public bool fUsefLockPosition;
    public bool fUsefLockRotation;
    public bool fUsefLockText;
    public bool fUsefLockVertices;
    public ProtectionBooleans() { }
    
    public ProtectionBooleans(uint entryOperand)
    {
        fLockAgainstGrouping = Utils.BitmaskToBool(entryOperand, 0x1);
        fLockAdjustHandles = Utils.BitmaskToBool(entryOperand, 0x2);
        fLockText = Utils.BitmaskToBool(entryOperand, 0x4);
        fLockVertices = Utils.BitmaskToBool(entryOperand, 0x8);
        
        fLockCropping = Utils.BitmaskToBool(entryOperand, 0x10);
        fLockAgainstSelect = Utils.BitmaskToBool(entryOperand, 0x20);
        fLockPosition = Utils.BitmaskToBool(entryOperand, 0x30);
        fLockAspectRatio = Utils.BitmaskToBool(entryOperand, 0x40);
        
        fLockRotation = Utils.BitmaskToBool(entryOperand, 0x100);
        fLockAgainstUngrouping = Utils.BitmaskToBool(entryOperand, 0x200);
        
        //unused 0x400 0x800 0x1000 0x2000 0x4000 0x8000
        
        fUsefLockAgainstGrouping = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefLockAdjustHandles = Utils.BitmaskToBool(entryOperand, 0x20000);
        fUsefLockText = Utils.BitmaskToBool(entryOperand, 0x40000);
        fUsefLockVertices = Utils.BitmaskToBool(entryOperand, 0x80000);
        
        fUsefLockCropping = Utils.BitmaskToBool(entryOperand, 0x100000);
        fUsefLockAgainstSelect = Utils.BitmaskToBool(entryOperand, 0x200000);
        fUsefLockPosition = Utils.BitmaskToBool(entryOperand, 0x400000);
        fUsefLockAspectRatio = Utils.BitmaskToBool(entryOperand, 0x800000);
        
        fUsefLockRotation = Utils.BitmaskToBool(entryOperand, 0x1000000);
        fUsefLockAgainstUngrouping = Utils.BitmaskToBool(entryOperand, 0x2000000);
    }
}