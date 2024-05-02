using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.Structures;

/// <summary>
///     Specifies an approximation of a real number, where the approximation has a fixed number of digits after the radix
///     point.
///     This type is specified in [MS-OSHARED] section 2.2.1.6.
///     Value of the real number = Integral + ( Fractional / 65536.0 )
///     Integral (2 bytes): A signed integer that specifies the integral part of the real number.
///     Fractional (2 bytes): An unsigned integer that specifies the fractional part of the real number.
/// </summary>
public class FixedPointNumber
{
    private readonly ushort fractional;
    private readonly ushort integral;
    
    public FixedPointNumber(ushort integral, ushort fractional)
    {
        this.integral = integral;
        this.fractional = fractional;
    }
    
    public FixedPointNumber(IStreamReader reader)
    {
        integral = reader.ReadUInt16();
        fractional = reader.ReadUInt16();
    }
    
    public double Value => integral + fractional / 65536.0d;
}