using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class GeometryTextBooleanProperties
{
    public bool fGtext;
    public bool fUsefGtext;
    public bool fUsegtextFBestFit;
    public bool fUsegtextFBold;
    public bool fUsegtextFDxMeasure;
    public bool fUsegtextFItalic;
    public bool fUsegtextFKern;
    public bool fUsegtextFNormalize;
    public bool fUsegtextFReverseRows;
    public bool fUsegtextFShadow;
    public bool fUsegtextFShrinkFit;
    public bool fUsegtextFSmallcaps;
    public bool fUsegtextFSStrikeThrough;
    public bool fUsegtextFStretch;
    public bool fUsegtextFTight;
    public bool fUsegtextFUnderline;
    public bool fUsegtextFVertical;
    public bool gtextFBestFit;
    public bool gtextFBold;
    public bool gtextFDxMeasure;
    public bool gtextFItalic;
    public bool gtextFKern;
    public bool gtextFNormalize;
    public bool gtextFReverseRows;
    public bool gtextFShadow;
    public bool gtextFShrinkFit;
    public bool gtextFSmallcaps;
    public bool gtextFStretch;
    public bool gtextFStrikethrough;
    public bool gtextFTight;
    public bool gtextFUnderline;
    public bool gtextFVertical;
    
    public GeometryTextBooleanProperties(uint entryOperand)
    {
        gtextFStrikethrough = Utils.BitmaskToBool(entryOperand, 0x1);
        gtextFSmallcaps = Utils.BitmaskToBool(entryOperand, 0x1 << 1);
        gtextFShadow = Utils.BitmaskToBool(entryOperand, 0x1 << 2);
        gtextFUnderline = Utils.BitmaskToBool(entryOperand, 0x1 << 3);
        gtextFItalic = Utils.BitmaskToBool(entryOperand, 0x1 << 4);
        gtextFBold = Utils.BitmaskToBool(entryOperand, 0x1 << 5);
        gtextFDxMeasure = Utils.BitmaskToBool(entryOperand, 0x1 << 6);
        gtextFNormalize = Utils.BitmaskToBool(entryOperand, 0x1 << 7);
        gtextFBestFit = Utils.BitmaskToBool(entryOperand, 0x1 << 8);
        gtextFShrinkFit = Utils.BitmaskToBool(entryOperand, 0x1 << 9);
        gtextFStretch = Utils.BitmaskToBool(entryOperand, 0x1 << 10);
        gtextFTight = Utils.BitmaskToBool(entryOperand, 0x1 << 11);
        gtextFKern = Utils.BitmaskToBool(entryOperand, 0x1 << 12);
        gtextFVertical = Utils.BitmaskToBool(entryOperand, 0x1 << 13);
        fGtext = Utils.BitmaskToBool(entryOperand, 0x1 << 14);
        gtextFReverseRows = Utils.BitmaskToBool(entryOperand, 0x1 << 15);
        
        fUsegtextFSStrikeThrough = Utils.BitmaskToBool(entryOperand, 0x1 << 16);
        fUsegtextFSmallcaps = Utils.BitmaskToBool(entryOperand, 0x1 << 17);
        fUsegtextFShadow = Utils.BitmaskToBool(entryOperand, 0x1 << 18);
        fUsegtextFUnderline = Utils.BitmaskToBool(entryOperand, 0x1 << 19);
        fUsegtextFItalic = Utils.BitmaskToBool(entryOperand, 0x1 << 20);
        fUsegtextFBold = Utils.BitmaskToBool(entryOperand, 0x1 << 21);
        fUsegtextFDxMeasure = Utils.BitmaskToBool(entryOperand, 0x1 << 22);
        fUsegtextFNormalize = Utils.BitmaskToBool(entryOperand, 0x1 << 23);
        fUsegtextFBestFit = Utils.BitmaskToBool(entryOperand, 0x1 << 24);
        fUsegtextFShrinkFit = Utils.BitmaskToBool(entryOperand, 0x1 << 25);
        fUsegtextFStretch = Utils.BitmaskToBool(entryOperand, 0x1 << 26);
        fUsegtextFTight = Utils.BitmaskToBool(entryOperand, 0x1 << 27);
        fUsegtextFKern = Utils.BitmaskToBool(entryOperand, 0x1 << 28);
        fUsegtextFVertical = Utils.BitmaskToBool(entryOperand, 0x1 << 29);
        fUsefGtext = Utils.BitmaskToBool(entryOperand, 0x1 << 30);
        fUsegtextFReverseRows = Utils.BitmaskToBool(entryOperand, 0x40000000);
    }
}