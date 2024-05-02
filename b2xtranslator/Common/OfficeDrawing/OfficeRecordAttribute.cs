using System;

namespace b2xtranslator.OfficeDrawing;

/// <summary>
///     Used for mapping Office record TypeCodes to the classes implementing them.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OfficeRecordAttribute : Attribute
{
    public OfficeRecordAttribute() { }
    
    public OfficeRecordAttribute(params ushort[] typecodes)
    {
        TypeCodes = typecodes;
    }
    
    public ushort[] TypeCodes { get; } = new ushort[0];
}