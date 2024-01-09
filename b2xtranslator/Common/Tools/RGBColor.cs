using System;

namespace b2xtranslator.Tools
{
    public class RGBColor
    {
        public enum ByteOrder 
        {
            RedFirst,
            RedLast
        }

        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;
        public string SixDigitHexCode;
        public string EightDigitHexCode;

        public RGBColor(int cv, ByteOrder order)
        {
            var bytes = System.BitConverter.GetBytes(cv);

            if(order == ByteOrder.RedFirst)
            {
                //R
                this.Red = bytes[0];
                this.SixDigitHexCode = $"{this.Red:x2}";
                //G
                this.Green = bytes[1];
                this.SixDigitHexCode += $"{this.Green:x2}";
                //B
                this.Blue = bytes[2];
                this.SixDigitHexCode += $"{this.Blue:x2}";
                this.EightDigitHexCode = this.SixDigitHexCode;
                //Alpha
                this.Alpha = bytes[3];
                this.EightDigitHexCode += $"{this.Alpha:x2}";
            }
            else if (order == ByteOrder.RedLast)
            {
                //R
                this.Red = bytes[2];
                this.SixDigitHexCode = $"{this.Red:x2}";
                //G
                this.Green = bytes[1];
                this.SixDigitHexCode += $"{this.Green:x2}";
                //B
                this.Blue = bytes[0];
                this.SixDigitHexCode += $"{this.Blue:x2}";
                this.EightDigitHexCode = this.SixDigitHexCode;
                //Alpha
                this.Alpha = bytes[3];
                this.EightDigitHexCode += $"{this.Alpha:x2}";
            }

        }
    }
}
