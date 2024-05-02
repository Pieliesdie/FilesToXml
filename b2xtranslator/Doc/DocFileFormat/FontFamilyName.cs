using System.IO;
using System.Text;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class FontFamilyName : ByteStructure
{
    /// <summary>
    ///     Character set identifier
    /// </summary>
    public byte chs;
    /// <summary>
    ///     Font family id
    /// </summary>
    public byte ff;
    /// <summary>
    ///     Font sinature
    /// </summary>
    public FontSignature fs;
    /// <summary>
    ///     When true, font is a TrueType font
    /// </summary>
    public bool fTrueType;
    /// <summary>
    ///     Panose
    /// </summary>
    public byte[] panose;
    /// <summary>
    ///     Pitch request
    /// </summary>
    public byte prq;
    /// <summary>
    ///     Base weight of font
    /// </summary>
    public short wWeight;
    /// <summary>
    ///     Alternative name of the font
    /// </summary>
    public string xszAlt;
    /// <summary>
    ///     Name of font
    /// </summary>
    public string xszFtn;
    
    public FontFamilyName(VirtualStreamReader reader, int length) : base(reader, length)
    {
        var startPos = _reader.BaseStream.Position;
        
        //FFID
        int ffid = _reader.ReadByte();
        
        var req = ffid;
        req = req << 6;
        req = req >> 6;
        prq = (byte)req;
        
        fTrueType = Utils.BitmaskToBool(ffid, 0x04);
        
        var family = ffid;
        family = family << 1;
        family = family >> 4;
        ff = (byte)family;
        
        wWeight = _reader.ReadInt16();
        
        chs = _reader.ReadByte();
        
        //skip byte 5
        _reader.ReadByte();
        
        //read the 10 bytes panose
        panose = _reader.ReadBytes(10);
        
        //read the 24 bytes FontSignature
        fs = new FontSignature
        {
            UnicodeSubsetBitfield0 = _reader.ReadUInt32(),
            UnicodeSubsetBitfield1 = _reader.ReadUInt32(),
            UnicodeSubsetBitfield2 = _reader.ReadUInt32(),
            UnicodeSubsetBitfield3 = _reader.ReadUInt32(),
            CodePageBitfield0 = _reader.ReadUInt32(),
            CodePageBitfield1 = _reader.ReadUInt32()
        };
        
        //read the next \0 terminated string
        var strStart = reader.BaseStream.Position;
        var strEnd = searchTerminationZero(_reader);
        xszFtn = Encoding.Unicode.GetString(_reader.ReadBytes((int)(strEnd - strStart)));
        xszFtn = xszFtn.Replace("\0", "");
        
        var readBytes = _reader.BaseStream.Position - startPos;
        if (readBytes < _length)
        {
            //read the next \0 terminated string
            strStart = reader.BaseStream.Position;
            strEnd = searchTerminationZero(_reader);
            xszAlt = Encoding.Unicode.GetString(_reader.ReadBytes((int)(strEnd - strStart)));
            xszAlt = xszAlt.Replace("\0", "");
        }
    }
    
    private long searchTerminationZero(VirtualStreamReader reader)
    {
        var strStart = reader.BaseStream.Position;
        while (reader.ReadInt16() != 0)
        {
            ;
        }
        
        var pos = reader.BaseStream.Position;
        reader.BaseStream.Seek(strStart, SeekOrigin.Begin);
        return pos;
    }
    
    public struct FontSignature
    {
        public uint UnicodeSubsetBitfield0;
        public uint UnicodeSubsetBitfield1;
        public uint UnicodeSubsetBitfield2;
        public uint UnicodeSubsetBitfield3;
        public uint CodePageBitfield0;
        public uint CodePageBitfield1;
    }
}