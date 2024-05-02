using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class TextBooleanProperties
{
    public bool fAutoTextMargin;
    public bool fFitShapeToText;
    public bool fSelectText;
    public bool fUsefAutoTextMargin;
    public bool fUsefFitShapeToText;
    public bool fUsefSelectText;
    
    public TextBooleanProperties(uint entryOperand)
    {
        //1 is unused
        fFitShapeToText = Utils.BitmaskToBool(entryOperand, 0x1 << 1);
        //1 is unused
        fAutoTextMargin = Utils.BitmaskToBool(entryOperand, 0x1 << 3);
        fSelectText = Utils.BitmaskToBool(entryOperand, 0x1 << 4);
        //12 unused
        fUsefFitShapeToText = Utils.BitmaskToBool(entryOperand, 0x1 << 17);
        //1 is unused
        fUsefAutoTextMargin = Utils.BitmaskToBool(entryOperand, 0x1 << 19);
        fUsefSelectText = Utils.BitmaskToBool(entryOperand, 0x1 << 20);
    }
}