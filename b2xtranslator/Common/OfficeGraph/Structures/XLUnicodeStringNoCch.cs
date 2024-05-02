using System.Text;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.Structures;

/// <summary>
///     This structure specifies a Unicode string.
///     When an XLUnicodeStringNoCch is used, the count of characters in the string
///     MUST be specified in the structure that uses the XLUnicodeStringNoCch.
/// </summary>
public class XLUnicodeStringNoCch
{
    /// <summary>
    ///     A bit that specifies whether the characters in rgb are double-byte characters.
    ///     MUST be a value from the following table:
    ///     Value   Meaning
    ///     0x0     All the characters in the string have a high byte of 0x00 and only the low bytes are in rgb.
    ///     0x1     All the characters in the string are saved as double-byte characters in rgb.
    /// </summary>
    public bool fHighByte;
    /// <summary>
    ///     An array of bytes that specifies the characters.
    ///     If fHighByte is 0x0, the size of the array MUST be equal to the count of characters in the string.
    ///     If fHighByte is 0x1, the size of the array MUST be equal to 2 times the count of characters in the string.
    /// </summary>
    public byte[] rgb;
    
    public XLUnicodeStringNoCch(IStreamReader reader, ushort cch)
    {
        fHighByte = Utils.BitmaskToBool(reader.ReadByte(), 0x0001);
        
        if (fHighByte)
        {
            rgb = new byte[2 * cch];
        }
        else
        {
            rgb = new byte[cch];
        }
        
        rgb = reader.ReadBytes(rgb.Length);
    }
    
    public string Value
    {
        get
        {
            if (fHighByte)
            {
                return Encoding.Unicode.GetString(rgb);
            }
            
            return Encoding.GetEncoding(1252).GetString(rgb);
        }
    }
}