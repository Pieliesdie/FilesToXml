using System;

namespace b2xtranslator.Tools;

public class RGBColor
{
    public enum ByteOrder
    {
        RedFirst,
        RedLast
    }
    
    public byte Alpha;
    public byte Blue;
    public string EightDigitHexCode;
    public byte Green;
    public byte Red;
    public string SixDigitHexCode;
    
    public RGBColor(int cv, ByteOrder order)
    {
        var bytes = BitConverter.GetBytes(cv);
        
        if (order == ByteOrder.RedFirst)
        {
            //R
            Red = bytes[0];
            SixDigitHexCode = $"{Red:x2}";
            //G
            Green = bytes[1];
            SixDigitHexCode += $"{Green:x2}";
            //B
            Blue = bytes[2];
            SixDigitHexCode += $"{Blue:x2}";
            EightDigitHexCode = SixDigitHexCode;
            //Alpha
            Alpha = bytes[3];
            EightDigitHexCode += $"{Alpha:x2}";
        }
        else if (order == ByteOrder.RedLast)
        {
            //R
            Red = bytes[2];
            SixDigitHexCode = $"{Red:x2}";
            //G
            Green = bytes[1];
            SixDigitHexCode += $"{Green:x2}";
            //B
            Blue = bytes[0];
            SixDigitHexCode += $"{Blue:x2}";
            EightDigitHexCode = SixDigitHexCode;
            //Alpha
            Alpha = bytes[3];
            EightDigitHexCode += $"{Alpha:x2}";
        }
    }
}