using System.IO;
using System.Text;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ListLevel : ByteStructure
{
    public enum FollowingChar
    {
        tab = 0,
        space,
        nothing
    }
    
    /// <summary>
    ///     Length, in bytes, of the LVL‘s grpprlChpx.
    /// </summary>
    public byte cbGrpprlChpx;
    /// <summary>
    ///     Length, in bytes, of the LVL‘s grpprlPapx.
    /// </summary>
    public byte cbGrpprlPapx;
    /// <summary>
    ///     Word 6.0 compatibility option: equivalent to anld.dxaIndent (see ANLD).<br />
    ///     Unused in newer versions.
    /// </summary>
    public int dxaIndent;
    /// <summary>
    ///     Word 6.0 compatibility option: equivalent to anld.dxaSpace (see ANLD). <br />
    ///     For newer versions indent to remove if we remove this numbering.
    /// </summary>
    public int dxaSpace;
    /// <summary>
    ///     True if the level turns all inherited numbers to arabic,
    ///     false if it preserves their number format code (nfc)
    /// </summary>
    public bool fLegal;
    /// <summary>
    ///     True if the level‘s number sequence is not restarted by
    ///     higher (more significant) levels in the list
    /// </summary>
    public bool fNoRestart;
    /// <summary>
    ///     Word 6.0 compatibility option: equivalent to anld.fPrev (see ANLD)
    /// </summary>
    public bool fPrev;
    /// <summary>
    ///     Word 6.0 compatibility option: equivalent to anld.fPrevSpace (see ANLD)
    /// </summary>
    public bool fPrevSpace;
    /// <summary>
    ///     True if this level was from a converted Word 6.0 document. <br />
    ///     If it is true, all of the Word 6.0 compatibility options become
    ///     valid otherwise they are ignored.
    /// </summary>
    public bool fWord6;
    /// <summary>
    ///     A grfhic that specifies HTML incompatibilities of the level.
    /// </summary>
    public byte grfhic;
    /// <summary>
    /// </summary>
    public CharacterPropertyExceptions grpprlChpx;
    /// <summary>
    /// </summary>
    public ParagraphPropertyExceptions grpprlPapx;
    /// <summary>
    ///     Limit of levels that we restart after.
    /// </summary>
    public byte ilvlRestartLim;
    /// <summary>
    ///     Start at value for this list level
    /// </summary>
    public int iStartAt;
    /// <summary>
    ///     The type of character following the number text for the paragraph.
    /// </summary>
    public FollowingChar ixchFollow;
    /// <summary>
    ///     Alignment (left, right, or centered) of the paragraph number.
    /// </summary>
    public byte jc;
    /// <summary>
    ///     Number format code (see anld.nfc for a list of options)
    /// </summary>
    public byte nfc;
    /// <summary>
    ///     Contains the character offsets into the LVL’s XST of the inherited numbers of previous levels. <br />
    ///     The XST contains place holders for any paragraph numbers contained in the text of the number,
    ///     and the place holder contains the ilvl of the inherited number,
    ///     so lvl.xst[lvl.rgbxchNums[0]] == the level of the first inherited number in this level.
    /// </summary>
    public byte[] rgbxchNums;
    /// <summary>
    /// </summary>
    public string xst;
    
    /// <summary>
    ///     Parses the given StreamReader to retrieve a LVL struct
    /// </summary>
    /// <param name="bytes"></param>
    public ListLevel(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        var startPos = _reader.BaseStream.Position;
        
        //parse the fix part
        iStartAt = _reader.ReadInt32();
        nfc = _reader.ReadByte();
        int flag = _reader.ReadByte();
        jc = (byte)(flag & 0x03);
        fLegal = Utils.BitmaskToBool(flag, 0x04);
        fNoRestart = Utils.BitmaskToBool(flag, 0x08);
        fPrev = Utils.BitmaskToBool(flag, 0x10);
        fPrevSpace = Utils.BitmaskToBool(flag, 0x20);
        fWord6 = Utils.BitmaskToBool(flag, 0x40);
        rgbxchNums = new byte[9];
        for (var i = 0; i < 9; i++)
        {
            rgbxchNums[i] = _reader.ReadByte();
        }
        
        ixchFollow = (FollowingChar)_reader.ReadByte();
        
        dxaSpace = _reader.ReadInt32();
        dxaIndent = _reader.ReadInt32();
        
        cbGrpprlChpx = _reader.ReadByte();
        cbGrpprlPapx = _reader.ReadByte();
        
        ilvlRestartLim = _reader.ReadByte();
        grfhic = _reader.ReadByte();
        
        //parse the variable part
        
        //read the group of papx sprms
        //this papx has no istd, so use PX to parse it
        var px = new PropertyExceptions(_reader.ReadBytes(cbGrpprlPapx));
        grpprlPapx = new ParagraphPropertyExceptions
        {
            grpprl = px.grpprl
        };
        
        //read the group of chpx sprms
        grpprlChpx = new CharacterPropertyExceptions(_reader.ReadBytes(cbGrpprlChpx));
        
        //read the number text
        var strLen = _reader.ReadInt16();
        xst = Encoding.Unicode.GetString(_reader.ReadBytes(strLen * 2));
        
        var endPos = _reader.BaseStream.Position;
        _reader.BaseStream.Seek(startPos, SeekOrigin.Begin);
        _rawBytes = _reader.ReadBytes((int)(endPos - startPos));
    }
}