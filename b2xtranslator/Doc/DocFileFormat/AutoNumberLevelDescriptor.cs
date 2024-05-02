using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class AutoNumberLevelDescriptor
{
    /// <summary>
    ///     24-bit color for Word 2000
    /// </summary>
    public int cv;
    /// <summary>
    ///     anld.cxchTextBefore will be the beginning offset of the text in
    ///     the anld.rgxch that will be displayed as the suffix of an auto number.
    ///     The sum of anld.cxchTextBefore + anld.cxchTextAfter will be the limit
    ///     of the auto number suffix in anld.rgxch
    /// </summary>
    public byte cxchTextAfter;
    /// <summary>
    ///     Offset into anld.rgxch that is the limit of the text that will be
    ///     displayed as the prefix of the auto number text
    /// </summary>
    public byte cxchTextBefore;
    /// <summary>
    ///     Width of prefix text (same as indent)
    /// </summary>
    public ushort dxaIndent;
    /// <summary>
    ///     Minimum space between number and paragraph
    /// </summary>
    public ushort dxaSpace;
    /// <summary>
    ///     Determines boldness of auto number when fSetBold is true
    /// </summary>
    public bool fBold;
    /// <summary>
    ///     Determines wheter auto number will be displayed using
    ///     caps when fSetCaps is true
    /// </summary>
    public bool fCaps;
    /// <summary>
    ///     When true, number will be displayed using hanging indent
    /// </summary>
    public bool fHang;
    /// <summary>
    ///     Determines italicness of auto number when fSetItalic is true
    /// </summary>
    public bool fItalic;
    /// <summary>
    ///     When true, number generated will include previous levels
    /// </summary>
    public bool fPrev;
    /// <summary>
    ///     When true, auto number will be displayed with a single
    ///     prefixing space character
    /// </summary>
    public bool fPrevSpace;
    /// <summary>
    ///     When true, boldness of number will be determined by fBold
    /// </summary>
    public bool fSetBold;
    /// <summary>
    ///     When true, fCaps will determine wheter number will be
    ///     displayed capitalized or not.
    /// </summary>
    public bool fSetCaps;
    /// <summary>
    ///     When true, italicness of number will be determined by fItalic
    /// </summary>
    public bool fSetItalic;
    /// <summary>
    ///     When true, kul will determine the underlining state of
    ///     the auto number
    /// </summary>
    public bool fSetKul;
    /// <summary>
    ///     When true, fSmallCaps will determine wheter number will be
    ///     displayed in small caps or not.
    /// </summary>
    public bool fSetSmallCaps;
    /// <summary>
    ///     When true, fStrike will determine wheter the number will be
    ///     displayed using strikethrough or not.
    /// </summary>
    public bool fSetStrike;
    /// <summary>
    ///     Determines wheter auto number will be displayed using
    ///     small caps when fSetSmallCaps is true
    /// </summary>
    public bool fSmallCaps;
    /// <summary>
    ///     Determines wheter auto number will be displayed using
    ///     caps when fSetStrike is true
    /// </summary>
    public bool fStrike;
    /// <summary>
    ///     Font code of auto number
    /// </summary>
    public short ftc;
    /// <summary>
    ///     Font half point size (0 = auto)
    /// </summary>
    public ushort hps;
    /// <summary>
    ///     Color of auto number for Word 97.<br />
    ///     Unused in Word 2000
    /// </summary>
    public byte ico;
    /// <summary>
    ///     Starting value
    /// </summary>
    public ushort iStartAt;
    /// <summary>
    ///     Justification code<br />
    ///     0 left justify<br />
    ///     1 center<br />
    ///     2 right justify<br />
    ///     3 left and right justfy
    /// </summary>
    public byte jc;
    /// <summary>
    ///     Determines wheter auto number will be displayed with
    ///     underlining when fSetKul is true
    /// </summary>
    public byte kul;
    /// <summary>
    ///     Number format code
    /// </summary>
    public byte nfc;
    
    /// <summary>
    ///     Creates a new AutoNumberedListDataDescriptor with default values
    /// </summary>
    public AutoNumberLevelDescriptor()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a AutoNumberLevelDescriptor
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public AutoNumberLevelDescriptor(byte[] bytes)
    {
        if (bytes.Length == 20)
        {
            nfc = bytes[0];
            cxchTextBefore = bytes[1];
            cxchTextAfter = bytes[2];
            int b3 = bytes[3];
            jc = Convert.ToByte(b3 & 0x03);
            fPrev = Utils.BitmaskToBool(b3, 0x04);
            fHang = Utils.BitmaskToBool(b3, 0x08);
            fSetBold = Utils.BitmaskToBool(b3, 0x10);
            fSetItalic = Utils.BitmaskToBool(b3, 0x20);
            fSetSmallCaps = Utils.BitmaskToBool(b3, 0x40);
            fSetCaps = Utils.BitmaskToBool(b3, 0x80);
            int b4 = bytes[4];
            fSetStrike = Utils.BitmaskToBool(b4, 0x01);
            fSetKul = Utils.BitmaskToBool(b4, 0x02);
            fPrevSpace = Utils.BitmaskToBool(b4, 0x04);
            fBold = Utils.BitmaskToBool(b4, 0x08);
            fItalic = Utils.BitmaskToBool(b4, 0x10);
            fSmallCaps = Utils.BitmaskToBool(b4, 0x20);
            fCaps = Utils.BitmaskToBool(b4, 0x40);
            fStrike = Utils.BitmaskToBool(b4, 0x80);
            int b5 = bytes[5];
            kul = (byte)(b5 & 0x07);
            ico = (byte)(b5 & 0xF1);
            ftc = BitConverter.ToInt16(bytes, 6);
            hps = BitConverter.ToUInt16(bytes, 8);
            iStartAt = BitConverter.ToUInt16(bytes, 10);
            dxaIndent = BitConverter.ToUInt16(bytes, 12);
            dxaSpace = BitConverter.ToUInt16(bytes, 14);
            cv = BitConverter.ToInt32(bytes, 16);
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct AutoNumberLevelDescriptor, the length of the struct doesn't match");
        }
    }
    
    private void setDefaultValues()
    {
        cv = 0;
        cxchTextAfter = 0;
        cxchTextBefore = 0;
        dxaIndent = 0;
        dxaSpace = 0;
        fBold = false;
        fCaps = false;
        fHang = false;
        fItalic = false;
        fPrev = false;
        fPrevSpace = false;
        fSetBold = false;
        fSetCaps = false;
        fSetItalic = false;
        fSetKul = false;
        fSetSmallCaps = false;
        fSetStrike = false;
        fSmallCaps = false;
        fStrike = false;
        ftc = 0;
        hps = 0;
        ico = 0;
        iStartAt = 0;
        jc = 0;
        kul = 0;
        nfc = 0;
    }
}