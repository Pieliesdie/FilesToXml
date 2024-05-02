using System.IO;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ListFormatOverrideLevel : ByteStructure
{
    private const int LFOLVL_LENGTH = 6;
    /// <summary>
    ///     True if the formatting is overridden
    /// </summary>
    public bool fFormatting;
    /// <summary>
    ///     True if the start-at value is overridden
    /// </summary>
    public bool fStartAt;
    /// <summary>
    ///     The level to be overridden
    /// </summary>
    public byte ilvl;
    /// <summary>
    ///     Start-at value if fFormatting==false and fStartAt==true.
    ///     If fFormatting == true, the start is stored in the LVL
    /// </summary>
    public int iStartAt;
    
    /// <summary>
    ///     Parses the bytes to retrieve a ListFormatOverrideLevel
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public ListFormatOverrideLevel(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        var startPos = _reader.BaseStream.Position;
        
        iStartAt = _reader.ReadInt32();
        int flag = _reader.ReadInt16();
        ilvl = (byte)(flag & 0x000F);
        fStartAt = Utils.BitmaskToBool(flag, 0x0010);
        fFormatting = Utils.BitmaskToBool(flag, 0x0020);
        
        //it's a complete override, so the fix part is followed by LVL struct
        if (fFormatting) { }
        
        _reader.BaseStream.Seek(startPos, SeekOrigin.Begin);
        _rawBytes = _reader.ReadBytes(LFOLVL_LENGTH);
    }
}