using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ParagraphHeight
{
    /// <summary>
    ///     When fDiffLines is 0, is number of lines in paragraph
    /// </summary>
    public short clMac;
    /// <summary>
    ///     If not == 0, used as a hint when finding the next row.<br />
    ///     (this value is only set if the PHE is stored in a PAP whose fTtp field is set)
    /// </summary>
    public short dcpTtpNext;
    /// <summary>
    ///     Width of lines in paragraph
    /// </summary>
    public int dxaCol;
    /// <summary>
    ///     When fDiffLines is true, is the total height in pixels of the paragraph
    /// </summary>
    public int dymHeight;
    /// <summary>
    ///     When fDiffLines is true, is height of every line in paragraph in pixels
    /// </summary>
    public int dymLine;
    /// <summary>
    ///     Height of table row.<br />
    ///     (this value is only set if the PHE is stored in a PAP whose fTtp field is set)
    /// </summary>
    public int dymTableHeight;
    /// <summary>
    ///     When true, total height of paragraph is known but lines in
    ///     paragraph have different heights
    /// </summary>
    public bool fDiffLines;
    /// <summary>
    ///     Reserved
    /// </summary>
    public bool fSpare;
    /// <summary>
    ///     ParagraphHeight is valid when fUnk is true
    /// </summary>
    public bool fUnk;
    /// <summary>
    ///     Complex shape layout in this paragraph
    /// </summary>
    public bool fVolatile;
    
    /// <summary>
    ///     Creates a new empty ParagraphHeight with default values
    /// </summary>
    public ParagraphHeight()
    {
        //set default values
        setDefaultValues();
    }
    
    /// <summary>
    ///     Parses the bytes to retrieve a ParagraphHeight
    /// </summary>
    /// <param name="bytes">The bytes</param>
    /// <param name="fTtpMode">
    ///     The flag which indicates if the
    ///     ParagraphHeight is stored in a ParagraphProperties whose fTtp field is set
    /// </param>
    public ParagraphHeight(byte[] bytes, bool fTtpMode)
    {
        //set default values
        setDefaultValues();
        
        if (bytes.Length == 12)
        {
            // The ParagraphHeight is placed in a ParagraphProperties whose fTtp field is set, 
            //so used another bit setting
            if (fTtpMode)
            {
                fSpare = Utils.BitmaskToBool(BitConverter.ToInt16(bytes, 0), 0x0001);
                fUnk = Utils.BitmaskToBool(BitConverter.ToInt16(bytes, 0), 0x0002);
                dcpTtpNext = BitConverter.ToInt16(bytes, 0);
                dxaCol = BitConverter.ToInt32(bytes, 4);
                dymTableHeight = BitConverter.ToInt32(bytes, 8);
            }
            else
            {
                fVolatile = Utils.BitmaskToBool(BitConverter.ToInt16(bytes, 0), 0x0001);
                fUnk = Utils.BitmaskToBool(BitConverter.ToInt16(bytes, 0), 0x0002);
                fDiffLines = Utils.BitmaskToBool(BitConverter.ToInt16(bytes, 0), 0x0004);
                clMac = Convert.ToInt16(BitConverter.ToUInt16(bytes, 0) & 0x00FF);
                
                dxaCol = BitConverter.ToInt32(bytes, 4);
                dymLine = BitConverter.ToInt32(bytes, 8);
                dymHeight = BitConverter.ToInt32(bytes, 8);
            }
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct ParagraphHeight, the length of the struct doesn't match");
        }
    }
    
    private void setDefaultValues()
    {
        clMac = 0;
        dcpTtpNext = 0;
        dxaCol = 0;
        dymHeight = 0;
        dymLine = 0;
        dymTableHeight = 0;
        fDiffLines = false;
        fSpare = false;
        fUnk = false;
        fVolatile = false;
    }
}