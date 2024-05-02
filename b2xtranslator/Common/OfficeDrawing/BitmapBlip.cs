using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF01D, 0xF01E, 0xF01F, 0xF020, 0xF021)]
public class BitmapBlip : Record
{
    /// <summary>
    /// </summary>
    public byte m_bTag;
    /// <summary>
    ///     Raster bits of the blip
    /// </summary>
    public byte[] m_pvBits;
    /// <summary>
    ///     The secondary, or data, UID - should always be set.
    /// </summary>
    public byte[] m_rgbUid;
    /// <summary>
    ///     The primary UID - this defaults to 0, in which case the primary ID is that of the internal data. <br />
    ///     NOTE!: The primary UID is only saved to disk if (blip_instance ^ blip_signature == 1). <br />
    ///     Blip_instance is MSOFBH.finst and blip_signature is one of the values defined in MSOBI
    /// </summary>
    public byte[] m_rgbUidPrimary;
    
    public BitmapBlip(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        m_rgbUid = Reader.ReadBytes(16);
        
        if (Instance == 0x6E1)
        {
            m_rgbUidPrimary = Reader.ReadBytes(16);
            m_bTag = Reader.ReadByte();
            m_pvBits = Reader.ReadBytes((int)(size - 33));
        }
        else
        {
            m_rgbUidPrimary = new byte[16];
            m_bTag = Reader.ReadByte();
            m_pvBits = Reader.ReadBytes((int)(size - 17));
        }
    }
}