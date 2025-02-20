using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class ListFormatOverride : ByteStructure
{
    private const int LFO_LENGTH = 16;
    /// <summary>
    ///     Count of levels whose format is overridden
    /// </summary>
    public byte clfolvl;
    /// <summary>
    ///     A grfhic that specifies HTML incompatibilities.
    /// </summary>
    public byte grfhic;
    /// <summary>
    ///     Specifies the field this LFO represents.
    ///     MUST be a value from the following table:<br />
    ///     0x00:   This LFO is not used for any field.<br />
    ///     0xFC:   This LFO is used for the AUTONUMLGL field.<br />
    ///     0xFD:   This LFO is used for the AUTONUMOUT field.<br />
    ///     0xFE:   This LFO is used for the AUTONUM field.<br />
    ///     0xFF:   This LFO is not used for any field.
    /// </summary>
    public byte ibstFltAutoNum;
    /// <summary>
    ///     List ID of corresponding ListData
    /// </summary>
    public int lsid;
    /// <summary>
    ///     Array of all levels whose format is overridden
    /// </summary>
    public ListFormatOverrideLevel[] rgLfoLvl;
    
    /// <summary>
    ///     Parses the given Stream Reader to retrieve a ListFormatOverride
    /// </summary>
    /// <param name="bytes">The bytes</param>
    public ListFormatOverride(VirtualStreamReader reader, int length) : base(reader, length)
    {
        var startPos = _reader.BaseStream.Position;
        
        lsid = _reader.ReadInt32();
        _reader.ReadBytes(8);
        clfolvl = _reader.ReadByte();
        ibstFltAutoNum = _reader.ReadByte();
        grfhic = _reader.ReadByte();
        _reader.ReadByte();
        
        rgLfoLvl = new ListFormatOverrideLevel[clfolvl];
        
        _reader.BaseStream.Seek(startPos, SeekOrigin.Begin);
        _rawBytes = _reader.ReadBytes(LFO_LENGTH);
    }
}