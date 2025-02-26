using System;
using System.Collections;
using System.Text;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class DocumentTypographyInfo
{
    /// <summary>
    ///     Length of rgxchFPunct
    /// </summary>
    public short cchFollowingPunct;
    /// <summary>
    ///     Length of rgxchLPunct
    /// </summary>
    public short cchLeadingPunct;
    /// <summary>
    ///     "2 page on 1" feature is turned on
    /// </summary>
    public bool f2on1;
    /// <summary>
    ///     When set to true, use strict (level 2) Kinsoku rules
    /// </summary>
    public bool fJapaneseUseLevel2;
    /// <summary>
    ///     True if we're kerning punctation
    /// </summary>
    public bool fKerningPunct;
    /// <summary>
    ///     Old East Asian feature
    /// </summary>
    public bool fOldDefineLineBaseOnGrid;
    /// <summary>
    ///     Custom Kinsoku
    /// </summary>
    public short iCustomKsu;
    /// <summary>
    ///     Kinsoku method of justification:<br />
    ///     0 = always expand<br />
    ///     1 = compress punctation<br />
    ///     2 = compress punctation and kana
    /// </summary>
    public short iJustification;
    /// <summary>
    ///     Level of kinsoku:<br />
    ///     0 = level 1<br />
    ///     1 = Level 2<br />
    ///     2 = Custom
    /// </summary>
    public short iLevelOfKinsoku;
    /// <summary>
    ///     Array of characters that should never appear at the start of a line
    /// </summary>
    public char[] rgxchFPunct;
    /// <summary>
    ///     Array of characters that should never appear at the end of a line
    /// </summary>
    public char[] rgxchLPunct;
    
    /// <summary>
    ///     Parses the bytes to retrieve a DocumentTypographyInfo
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public DocumentTypographyInfo(byte[] bytes)
    {
        if (bytes.Length == 310)
        {
            BitArray bits;
            
            //split byte 0 and 1 into bits
            bits = new BitArray(new[] { bytes[0], bytes[1] });
            fKerningPunct = bits[0];
            iJustification = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 1, 2));
            iLevelOfKinsoku = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 3, 2));
            f2on1 = bits[5];
            fOldDefineLineBaseOnGrid = bits[6];
            iCustomKsu = (short)Utils.BitArrayToUInt32(Utils.BitArrayCopy(bits, 7, 3));
            fJapaneseUseLevel2 = bits[10];
            
            cchFollowingPunct = BitConverter.ToInt16(bytes, 2);
            cchLeadingPunct = BitConverter.ToInt16(bytes, 4);
            
            var fpunctBytes = new byte[202];
            Array.Copy(bytes, 6, fpunctBytes, 0, fpunctBytes.Length);
            rgxchFPunct = Encoding.Unicode.GetString(fpunctBytes).ToCharArray();
            
            var lpunctBytes = new byte[102];
            Array.Copy(bytes, 208, lpunctBytes, 0, lpunctBytes.Length);
            rgxchLPunct = Encoding.Unicode.GetString(lpunctBytes).ToCharArray();
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct DOPTYPOGRAPHY, the length of the struct doesn't match");
        }
    }
}