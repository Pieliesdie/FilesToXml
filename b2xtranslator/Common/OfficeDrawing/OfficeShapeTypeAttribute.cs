using System;

namespace b2xtranslator.OfficeDrawing;

/// <summary>
///     Used for mapping Office shape types to the classes implementing them.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OfficeShapeTypeAttribute : Attribute
{
    public uint TypeCode;
    public OfficeShapeTypeAttribute() { }
    
    public OfficeShapeTypeAttribute(uint typecode)
    {
        TypeCode = typecode;
    }
}