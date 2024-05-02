using System.IO;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ListData : ByteStructure
{
    public const short ISTD_NIL = 4095;
    private const int LSTF_LENGTH = 28;
    /// <summary>
    ///     To emulate Word 6.0 numbering: <br />
    ///     True if Auto numbering
    /// </summary>
    public bool fAutoNum;
    /// <summary>
    ///     When true, list is a hybrid multilevel/simple (UI=simple, internal=multilevel)
    /// </summary>
    public bool fHybrid;
    /// <summary>
    ///     When true, this list was there before we started reading RTF.
    /// </summary>
    public bool fPreRTF;
    /// <summary>
    ///     Word 6.0 compatibility option:<br />
    ///     True if the list should start numbering over at the beginning of each section.
    /// </summary>
    public bool fRestartHdn;
    /// <summary>
    ///     True if this is a simple (one-level) list.<br />
    ///     False if this is a multilevel (nine-level) list.
    /// </summary>
    public bool fSimpleList;
    /// <summary>
    ///     A grfhic that specifies HTML incompatibilities of the list.
    /// </summary>
    public byte grfhic;
    /// <summary>
    ///     Unique List ID
    /// </summary>
    public int lsid;
    /// <summary>
    ///     Array of shorts containing the istd‘s linked to each level of the list,
    ///     or ISTD_NIL (4095) if no style is linked.
    /// </summary>
    public short[] rgistd;
    /// <summary>
    ///     Array of ListLevel describing the several levels of the list.
    /// </summary>
    public ListLevel[] rglvl;
    /// <summary>
    ///     Unique template code
    /// </summary>
    public int tplc;
    
    /// <summary>
    ///     Parses the StreamReader to retrieve a ListData
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public ListData(VirtualStreamReader reader, int length) : base(reader, length)
    {
        var startPos = _reader.BaseStream.Position;
        
        lsid = _reader.ReadInt32();
        tplc = _reader.ReadInt32();
        
        rgistd = new short[9];
        for (var i = 0; i < 9; i++)
        {
            rgistd[i] = _reader.ReadInt16();
        }
        
        //parse flagbyte
        int flag = _reader.ReadByte();
        fSimpleList = Utils.BitmaskToBool(flag, 0x01);
        
        if (fSimpleList)
        {
            rglvl = new ListLevel[1];
        }
        else
        {
            rglvl = new ListLevel[9];
        }
        
        fRestartHdn = Utils.BitmaskToBool(flag, 0x02);
        fAutoNum = Utils.BitmaskToBool(flag, 0x04);
        fPreRTF = Utils.BitmaskToBool(flag, 0x08);
        fHybrid = Utils.BitmaskToBool(flag, 0x10);
        
        grfhic = _reader.ReadByte();
        
        _reader.BaseStream.Seek(startPos, SeekOrigin.Begin);
        _rawBytes = _reader.ReadBytes(LSTF_LENGTH);
    }
}