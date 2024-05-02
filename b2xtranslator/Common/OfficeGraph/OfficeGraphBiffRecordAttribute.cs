using System;

namespace b2xtranslator.OfficeGraph;

/// <summary>
///     Used for mapping Office record TypeCodes to the classes implementing them.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OfficeGraphBiffRecordAttribute : Attribute
{
    public OfficeGraphBiffRecordAttribute() { }
    
    public OfficeGraphBiffRecordAttribute(params GraphRecordNumber[] typecodes)
    {
        TypeCodes = typecodes;
    }
    
    public GraphRecordNumber[] TypeCodes { get; } = new GraphRecordNumber[0];
}