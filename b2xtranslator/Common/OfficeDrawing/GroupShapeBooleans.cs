using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class GroupShapeBooleans
{
    public bool fAllowOverlap;
    public bool fBehindDocument;
    public bool fEditedWrap;
    public bool fHidden;
    public bool fHorizRule;
    public bool fIsBullet;
    public bool fIsButton;
    public bool fLayoutInCell;
    public bool fNoshadeHR;
    public bool fOnDblClickNotify;
    public bool fOneD;
    public bool fPrint;
    public bool fReallyHidden;
    public bool fScriptAnchor;
    public bool fStandardHR;
    public bool fUsefAllowOverlap;
    public bool fUsefBehindDocument;
    public bool fUsefEditedWrap;
    public bool fUsefHidden;
    public bool fUsefHorizRule;
    public bool fUsefIsBullet;
    public bool fUsefIsButton;
    public bool fUsefLayoutInCell;
    public bool fUsefNoshadeHR;
    public bool fUsefOnDblClickNotify;
    public bool fUsefOneD;
    public bool fUsefPrint;
    public bool fUsefReallyHidden;
    public bool fUsefScriptAnchor;
    public bool fUsefStandardHR;
    public bool fUsefUserDrawn;
    public bool fUserDrawn;
    
    public GroupShapeBooleans(uint entryOperand)
    {
        fPrint = Utils.BitmaskToBool(entryOperand, 0x1);
        fHidden = Utils.BitmaskToBool(entryOperand, 0x2);
        fOneD = Utils.BitmaskToBool(entryOperand, 0x4);
        fIsButton = Utils.BitmaskToBool(entryOperand, 0x8);
        
        fOnDblClickNotify = Utils.BitmaskToBool(entryOperand, 0x10);
        fBehindDocument = Utils.BitmaskToBool(entryOperand, 0x20);
        fEditedWrap = Utils.BitmaskToBool(entryOperand, 0x40);
        fScriptAnchor = Utils.BitmaskToBool(entryOperand, 0x80);
        
        fReallyHidden = Utils.BitmaskToBool(entryOperand, 0x100);
        fAllowOverlap = Utils.BitmaskToBool(entryOperand, 0x200);
        fUserDrawn = Utils.BitmaskToBool(entryOperand, 0x400);
        fHorizRule = Utils.BitmaskToBool(entryOperand, 0x800);
        
        fNoshadeHR = Utils.BitmaskToBool(entryOperand, 0x1000);
        fStandardHR = Utils.BitmaskToBool(entryOperand, 0x2000);
        fIsBullet = Utils.BitmaskToBool(entryOperand, 0x4000);
        fLayoutInCell = Utils.BitmaskToBool(entryOperand, 0x8000);
        
        fUsefPrint = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefHidden = Utils.BitmaskToBool(entryOperand, 0x20000);
        fUsefOneD = Utils.BitmaskToBool(entryOperand, 0x40000);
        fUsefIsButton = Utils.BitmaskToBool(entryOperand, 0x80000);
        
        fUsefOnDblClickNotify = Utils.BitmaskToBool(entryOperand, 0x100000);
        fUsefBehindDocument = Utils.BitmaskToBool(entryOperand, 0x200000);
        fUsefEditedWrap = Utils.BitmaskToBool(entryOperand, 0x400000);
        fUsefScriptAnchor = Utils.BitmaskToBool(entryOperand, 0x800000);
        
        fUsefReallyHidden = Utils.BitmaskToBool(entryOperand, 0x1000000);
        fUsefAllowOverlap = Utils.BitmaskToBool(entryOperand, 0x2000000);
        fUsefUserDrawn = Utils.BitmaskToBool(entryOperand, 0x4000000);
        fUsefHorizRule = Utils.BitmaskToBool(entryOperand, 0x8000000);
        
        fUsefNoshadeHR = Utils.BitmaskToBool(entryOperand, 0x10000000);
        fUsefStandardHR = Utils.BitmaskToBool(entryOperand, 0x20000000);
        fUsefIsBullet = Utils.BitmaskToBool(entryOperand, 0x40000000);
        fUsefLayoutInCell = Utils.BitmaskToBool(entryOperand, 0x80000000);
    }
}