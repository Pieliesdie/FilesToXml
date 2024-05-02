using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class ShadowStyleBooleanProperties
{
    public bool fShadow;
    public bool fShadowObscured;
    public bool fUsefShadow;
    public bool fUsefshadowObscured;
    
    public ShadowStyleBooleanProperties(uint entryOperand)
    {
        fShadowObscured = Utils.BitmaskToBool(entryOperand, 0x1);
        fShadow = Utils.BitmaskToBool(entryOperand, 0x2);
        fUsefshadowObscured = Utils.BitmaskToBool(entryOperand, 0x10000);
        fUsefShadow = Utils.BitmaskToBool(entryOperand, 0x20000);
    }
}