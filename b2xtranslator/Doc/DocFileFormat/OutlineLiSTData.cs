using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class OutlineLiSTData
{
    /// <summary>
    ///     When true, restart heading on section break
    /// </summary>
    public bool fRestartHdr;
    /// <summary>
    ///     Reserved
    /// </summary>
    public bool fSpareOlst2;
    /// <summary>
    ///     Reserved
    /// </summary>
    public bool fSpareOlst3;
    /// <summary>
    ///     Reserved
    /// </summary>
    public bool fSpareOlst4;
    /// <summary>
    ///     An array of  ANLV structures describing how heading numbers
    ///     should be displayed fpr each of Word's 0 outline heading levels
    /// </summary>
    public AutoNumberLevelDescriptor[] rganlv;
    /// <summary>
    ///     Text before/after number
    /// </summary>
    public char[] rgxch;
    
    /// <summary>
    ///     Creates a new OutlineLiSTData with default values
    /// </summary>
    public OutlineLiSTData()
    {
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a OutlineLiSTData
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public OutlineLiSTData(byte[] bytes)
    {
        if (bytes.Length == 248)
        {
            //Fill the rganlv array
            var j = 0;
            for (var i = 0; i < 180; i += 20)
            {
                //copy the 20 byte pages
                var page = new byte[20];
                Array.Copy(bytes, i, page, 0, 20);
                rganlv[j] = new AutoNumberLevelDescriptor(page);
                j++;
            }
            
            //Set the flags
            fRestartHdr = Utils.IntToBool(bytes[180]);
            fSpareOlst2 = Utils.IntToBool(bytes[181]);
            fSpareOlst3 = Utils.IntToBool(bytes[182]);
            fSpareOlst4 = Utils.IntToBool(bytes[183]);
            
            //Fill the rgxch array
            j = 0;
            for (var i = 184; i < 64; i += 2)
            {
                rgxch[j] = Convert.ToChar(BitConverter.ToInt16(bytes, i));
                j++;
            }
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct OLST, the length of the struct doesn't match");
        }
    }
    
    private void setDefaultValues()
    {
        fRestartHdr = false;
        fSpareOlst2 = false;
        fSpareOlst3 = false;
        fSpareOlst4 = false;
        rganlv = new AutoNumberLevelDescriptor[9];
        for (var i = 0; i < 9; i++)
        {
            rganlv[i] = new AutoNumberLevelDescriptor();
        }
        
        rgxch = Utils.ClearCharArray(new char[32]);
    }
}