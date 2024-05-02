using System.Drawing;
using System.IO;
using System.IO.Compression;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF01A, 0xF01B, 0xF01C)]
public class MetafilePictBlip : Record
{
    public enum BlipCompression
    {
        Deflate,
        None = 254,
        Test = 255
    }
    
    /// <summary>
    ///     Cache of the metafile size
    /// </summary>
    public int m_cb;
    /// <summary>
    ///     Cache of saved size (size of m_pvBits)
    /// </summary>
    public int m_cbSave;
    /// <summary>
    ///     Compression
    /// </summary>
    public BlipCompression m_fCompression;
    /// <summary>
    ///     always msofilterNone
    /// </summary>
    public bool m_fFilter;
    /// <summary>
    ///     Boundary of metafile drawing commands
    /// </summary>
    public Point m_ptSize;
    /// <summary>
    ///     Compressed bits of metafile.
    /// </summary>
    public byte[] m_pvBits;
    public Rectangle m_rcBounds;
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
    
    public MetafilePictBlip(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        m_rgbUid = Reader.ReadBytes(16);
        m_rgbUidPrimary = new byte[16];
        m_cb = Reader.ReadInt32();
        
        m_rcBounds = new Rectangle(
            Reader.ReadInt32(),
            Reader.ReadInt32(),
            Reader.ReadInt32(),
            Reader.ReadInt32());
        
        m_ptSize = new Point(Reader.ReadInt32(), Reader.ReadInt32());
        
        m_cbSave = Reader.ReadInt32();
        m_fCompression = (BlipCompression)Reader.ReadByte();
        m_fFilter = Utils.ByteToBool(Reader.ReadByte());
        
        m_pvBits = Reader.ReadBytes(m_cbSave);
    }
    
    /// <summary>
    ///     Decompresses the bits of the picture if the picture is decompressed.<br />
    ///     If the picture is not compressed, it returns original byte array.
    /// </summary>
    public byte[] Decrompress()
    {
        if (m_fCompression == BlipCompression.Deflate)
        {
            //skip the first two bytes because the can not be interpreted by the DeflateStream
            var inStream = new DeflateStream(
                new MemoryStream(m_pvBits, 2, m_pvBits.Length - 2),
                CompressionMode.Decompress,
                false);
            
            var buffer = new byte[m_cb];
            inStream.Read(buffer, 0, m_cb);
            
            return buffer;
        }
        
        return m_pvBits;
    }
}