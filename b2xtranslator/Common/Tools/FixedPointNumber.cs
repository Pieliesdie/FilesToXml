using System;

namespace b2xtranslator.Tools;

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
    public ushort Fractional;
    public ushort Integral;
    
    public FixedPointNumber(ushort integral, ushort fractional)
    {
        Integral = integral;
        Fractional = fractional;
    }
    
    public FixedPointNumber(uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        Integral = BitConverter.ToUInt16(bytes, 0);
        Fractional = BitConverter.ToUInt16(bytes, 2);
    }
    
    public FixedPointNumber(byte[] bytes)
    {
        Integral = BitConverter.ToUInt16(bytes, 0);
        Fractional = BitConverter.ToUInt16(bytes, 2);
    }
    
    public double Value => Integral + Fractional / 65536.0d;
    
    //public FixedPointNumber(IStreamReader reader)
    //{
    //    this.integral = reader.ReadUInt16();
    //    this.fractional = reader.ReadUInt16();
    //}
    
    public double ToAngle()
    {
        if (Fractional != 0)
        {
            // negative angle
            return Fractional - 65536.0;
        }
        
        if (Integral != 0)
        {
            //positive angle
            return 65536.0 - Integral;
        }
        
        return 0.0;
    }
}