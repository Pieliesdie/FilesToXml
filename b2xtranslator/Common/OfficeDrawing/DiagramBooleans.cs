using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class DiagramBooleans
{
    public bool fDoFormat;
    public bool fDoLayout;
    public bool fPseudoInline;
    public bool fReverse;
    public bool fUsefDoFormat;
    public bool fUsefDoLayout;
    public bool fUsefPseudoInline;
    public bool fUsefReverse;
    
    public DiagramBooleans(uint entryOperand)
    {
        fPseudoInline = Utils.BitmaskToBool(entryOperand, 0x1);
        fDoLayout = Utils.BitmaskToBool(entryOperand, 0x2);
        fReverse = Utils.BitmaskToBool(entryOperand, 0x4);
        fDoFormat = Utils.BitmaskToBool(entryOperand, 0x8);
        
        //unused: 0x10 - 0x8000
        
        fUsefPseudoInline = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefDoLayout = Utils.BitmaskToBool(entryOperand, 0x20000);
        fUsefReverse = Utils.BitmaskToBool(entryOperand, 0x40000);
        fUsefDoFormat = Utils.BitmaskToBool(entryOperand, 0x80000);
    }
}