using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class NumberRevisionMarkData
{
    /// <summary>
    ///     Date of the paragraph number change
    /// </summary>
    public DateAndTime dttmNumRM;
    /// <summary>
    ///     True if this paragraph was numbered when revision
    ///     mark tracking was turned on
    /// </summary>
    public bool fNumRM;
    /// <summary>
    ///     Index of author IDs stored in hsttbfRMark for the
    ///     paragraph number change
    /// </summary>
    public short ibstNumRM;
    /// <summary>
    ///     Numeric value for each place holder in xst
    /// </summary>
    public int[] PNBR;
    /// <summary>
    ///     Index into xst of the locations of paragraph number
    ///     place holders for each level
    /// </summary>
    public char[] rgbxchNums;
    /// <summary>
    ///     Number format code for the paragraph number
    ///     place holders for each level
    /// </summary>
    public char[] rgnfc;
    /// <summary>
    ///     The text string for the paragraph number,
    ///     containing level place holders
    /// </summary>
    public char[] xst;
    
    /// <summary>
    ///     Creates a new NumberRevisionMarkData with default values
    /// </summary>
    public NumberRevisionMarkData()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a NumberRevisionMarkData
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public NumberRevisionMarkData(byte[] bytes)
    {
        if (bytes.Length == 128)
        {
            fNumRM = Utils.IntToBool(bytes[0]);
            ibstNumRM = BitConverter.ToInt16(bytes, 2);
            
            //copy date to new array and parse it
            var dttm = new byte[4];
            Array.Copy(bytes, 4, dttm, 0, 4);
            dttmNumRM = new DateAndTime(dttm);
            
            //fill the rgbxchNums char array
            rgbxchNums = new char[9];
            var j = 0;
            for (var i = 8; i < 17; i++)
            {
                rgbxchNums[j] = Convert.ToChar(bytes[i]);
                j++;
            }
            
            //fill the rgnfc char array
            rgnfc = new char[9];
            j = 0;
            for (var i = 17; i < 26; i++)
            {
                rgnfc[j] = Convert.ToChar(bytes[i]);
                j++;
            }
            
            //fill the PNBR array
            PNBR = new int[9];
            j = 0;
            for (var i = 28; i < 64; i += 4)
            {
                PNBR[j] = BitConverter.ToInt32(bytes, i);
                j++;
            }
            
            //fill the xst char array
            xst = new char[32];
            j = 0;
            for (var i = 64; i < 128; i += 2)
            {
                xst[j] = Convert.ToChar(BitConverter.ToUInt16(bytes, i));
                j++;
            }
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct NUMRM, the length of the struct doesn't match");
        }
    }
    
    private void setDefaultValues()
    {
        dttmNumRM = new DateAndTime();
        fNumRM = false;
        ibstNumRM = 0;
        PNBR = Utils.ClearIntArray(new int[9]);
        rgbxchNums = Utils.ClearCharArray(new char[9]);
        rgnfc = Utils.ClearCharArray(new char[9]);
        xst = Utils.ClearCharArray(new char[32]);
    }
}