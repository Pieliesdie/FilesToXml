using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which encapsulates methods which ease writing structured storage components to a stream.
///     Author: math
/// </summary>
internal class OutputHandler : AbstractIOHandler
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="memoryStream">The target memory stream.</param>
    internal OutputHandler(MemoryStream memoryStream)
    {
        _stream = memoryStream;
        _bitConverter = new InternalBitConverter(true);
    }
    
    internal Stream BaseStream => _stream;
    
    /// <summary>
    ///     Returns UInt64.MaxValue because size of stream is not defined yet.
    /// </summary>
    internal override ulong IOStreamSize => ulong.MaxValue;
    
    /// <summary>
    ///     Writes a byte to the stream.
    /// </summary>
    /// <param name="value">The byte to write.</param>
    internal void writeByte(byte value)
    {
        _stream.WriteByte(value);
    }
    
    /// <summary>
    ///     Writes a UInt16 to the stream.
    /// </summary>
    /// <param name="value">The UInt16 to write.</param>
    internal void writeUInt16(ushort value)
    {
        _stream.Write(_bitConverter.getBytes(value), 0, 2);
    }
    
    /// <summary>
    ///     Writes a UInt32 to the stream.
    /// </summary>
    /// <param name="value">The UInt32 to write.</param>
    internal void writeUInt32(uint value)
    {
        _stream.Write(_bitConverter.getBytes(value), 0, 4);
    }
    
    /// <summary>
    ///     Writes a UInt64 to the stream.
    /// </summary>
    /// <param name="value">The UInt64 to write.</param>
    internal void writeUInt64(ulong value)
    {
        _stream.Write(_bitConverter.getBytes(value), 0, 8);
    }
    
    /// <summary>
    ///     Writes a byte array to the stream.
    /// </summary>
    /// <param name="value">The byte array to write.</param>
    internal void write(byte[] data)
    {
        _stream.Write(data, 0, data.Length);
    }
    
    /// <summary>
    ///     Writes sectors to the stream and padding the sector with the given byte.
    /// </summary>
    /// <param name="data">The data to write.</param>
    /// <param name="sectorSize">The size of a sector.</param>
    /// <param name="padding">The byte which is used for padding</param>
    internal void writeSectors(byte[] data, ushort sectorSize, byte padding)
    {
        var remaining = (uint)(data.LongLength % sectorSize);
        _stream.Write(data, 0, data.Length);
        if (remaining == 0)
        {
            return;
        }
        
        for (uint i = 0; i < sectorSize - remaining; i++)
        {
            _stream.WriteByte(padding);
        }
    }
    
    /// <summary>
    ///     Writes sectors to the stream and padding the sector with the given UInt32.
    /// </summary>
    /// <param name="data">The data to write.</param>
    /// <param name="sectorSize">The size of a sector.</param>
    /// <param name="padding">The UInt32 which is used for padding</param>
    internal void writeSectors(byte[] data, ushort sectorSize, uint padding)
    {
        var remaining = (uint)(data.LongLength % sectorSize);
        _stream.Write(data, 0, data.Length);
        if (remaining == 0)
        {
            return;
        }
        
        // consistency check
        if ((sectorSize - remaining) % sizeof(uint) != 0)
        {
            throw new InvalidSectorSizeException();
        }
        
        for (uint i = 0; i < (sectorSize - remaining) / sizeof(uint); i++)
        {
            writeUInt32(padding);
        }
    }
    
    /// <summary>
    ///     Writes the internal memory stream to a given stream.
    /// </summary>
    /// <param name="stream">The output stream.</param>
    internal void writeToStream(Stream stream)
    {
        const int bytesToReadAtOnce = 512;
        
        var reader = new BinaryReader(BaseStream);
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        while (true)
        {
            var array = reader.ReadBytes(bytesToReadAtOnce);
            stream.Write(array, 0, array.Length);
            if (array.Length != bytesToReadAtOnce)
            {
                break;
            }
        }
        
        stream.Flush();
    }
}