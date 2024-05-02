using System;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Provides methods for accessing the file stream
///     Author: math
/// </summary>
internal class InputHandler : AbstractIOHandler
{
    /// <summary>
    ///     Constructor, opens the given file
    /// </summary>
    public InputHandler(string fileName)
    {
        _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
    }
    
    public InputHandler(Stream stream)
    {
        _stream = stream;
    }
    
    internal Stream _InternalFileStream => _stream;
    
    /// <summary>Get the size of the associated stream in bytes</summary>
    internal override ulong IOStreamSize => (ulong)_stream.Length;
    
    /// <summary>
    ///     Seeks relative to the current position by the given offset
    /// </summary>
    internal long RelativeSeek(long offset)
    {
        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset cannot be less than 0.");
        }
        
        return _stream.Seek(offset, SeekOrigin.Current);
    }
    
    /// <summary>
    ///     Seeks to a given sector in the compound file.
    ///     May only be used after SetHeaderReference() is called.
    /// </summary>
    internal long SeekToSector(long sector)
    {
        if (_header == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (sector < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sector), sector, "Sector cannot be less than 0.");
        }
        
        // header sector == -1
        if (sector == -1)
        {
            return _stream.Seek(0, SeekOrigin.Begin);
        }
        
        return _stream.Seek((sector << _header.SectorShift) + Measures.HeaderSize, SeekOrigin.Begin);
    }
    
    /// <summary>
    ///     Seeks to a given sector and position in the compound file.
    ///     May only be used after SetHeaderReference() is called.
    /// </summary>
    /// <returns>The new position in the stream.</returns>
    internal long SeekToPositionInSector(long sector, long position)
    {
        if (_header == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (position < 0 || position >= _header.SectorSize)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position less than 0 or greater than the header sector.");
        }
        
        // header sector == -1
        if (sector == -1)
        {
            return _stream.Seek(position, SeekOrigin.Begin);
        }
        
        return _stream.Seek((sector << _header.SectorShift) + Measures.HeaderSize + position, SeekOrigin.Begin);
    }
    
    /// <summary>
    ///     Reads a byte at the current position of the file stream.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The byte value read from the stream. </returns>
    internal byte ReadByte()
    {
        var result = _stream.ReadByte();
        if (result == -1)
        {
            throw new ReadBytesAmountMismatchException();
        }
        
        return (byte)result;
    }
    
    /// <summary>
    ///     Reads bytes at the current position of the file stream into a byte array.
    ///     The array size determines the number of bytes to read.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    internal void Read(byte[] array)
    {
        Read(array, 0, array.Length);
    }
    
    /// <summary>
    ///     Reads bytes at the current position of the file stream into a byte array.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <param name="array">The array to read to</param>
    /// <param name="offset">The offset in the array to read to</param>
    /// <param name="count">The number of bytes to read</param>
    internal void Read(byte[] array, int offset, int count)
    {
        var result = _stream.Read(array, offset, count);
        if (result != count)
        {
            throw new ReadBytesAmountMismatchException();
        }
    }
    
    /// <summary>
    ///     Reads a byte at the current position of the file stream.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The byte cast to an int, or -1 if reading from the end of the stream.</returns>
    internal int UncheckedReadByte()
    {
        return _stream.ReadByte();
    }
    
    /// <summary>
    ///     Reads bytes at the current position of the file stream into a byte array.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <param name="array">The array to read to</param>
    /// <param name="offset">The offset in the array to read to</param>
    /// <param name="count">The number of bytes to read</param>
    /// <returns>
    ///     The total number of bytes read into the buffer.
    ///     This might be less than the number of bytes requested if that number
    ///     of bytes are not currently available, or zero if the end of the stream is reached.
    /// </returns>
    internal int UncheckedRead(byte[] array, int offset, int count)
    {
        return _stream.Read(array, offset, count);
    }
    
    /// <summary>
    ///     Reads bytes at the given position of the file stream into a byte array.
    ///     The array size determines the number of bytes to read.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    internal void ReadPosition(byte[] array, long position)
    {
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position cannot be less than 0.");
        }
        
        _stream.Seek(position, 0);
        var result = _stream.Read(array, 0, array.Length);
        if (result != array.Length)
        {
            throw new ReadBytesAmountMismatchException();
        }
    }
    
    /// <summary>
    ///     Reads a UInt16 at the current position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt16 value read from the stream.</returns>
    internal ushort ReadUInt16()
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        var array = new byte[2];
        Read(array);
        return _bitConverter.ToUInt16(array);
    }
    
    /// <summary>
    ///     Reads a UInt32 at the current position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt32 value read from the stream.</returns>
    internal uint ReadUInt32()
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        var array = new byte[4];
        Read(array);
        return _bitConverter.ToUInt32(array);
    }
    
    /// <summary>
    ///     Reads a UInt64 at the current position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt64 value read from the stream.</returns>
    internal ulong ReadUInt64()
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        var array = new byte[8];
        Read(array);
        return _bitConverter.ToUInt64(array);
    }
    
    /// <summary>
    ///     Reads a UInt16 at the given position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt16 value read at the given position.</returns>
    internal ushort ReadUInt16(long position)
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position cannot be less than 0.");
        }
        
        var array = new byte[2];
        ReadPosition(array, position);
        return _bitConverter.ToUInt16(array);
    }
    
    /// <summary>
    ///     Reads a UInt32 at the given position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt32 value read at the given position.</returns>
    internal uint ReadUInt32(long position)
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position cannot be less than 0.");
        }
        
        var array = new byte[4];
        ReadPosition(array, position);
        return _bitConverter.ToUInt32(array);
    }
    
    /// <summary>
    ///     Reads a UInt64 at the given position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <returns>The UInt64 value read at the given position.</returns>
    internal ulong ReadUInt64(long position)
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(position), position, "Position cannot be less than 0.");
        }
        
        var array = new byte[8];
        ReadPosition(array, position);
        return _bitConverter.ToUInt64(array);
    }
    
    /// <summary>
    ///     Reads a UTF-16 encoded unicode string at the current position of the file stream.
    ///     May only be used after InitBitConverter() is called.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    /// <param name="size">The maximum size of the string in bytes (1 char = 2 bytes) including the Unicode NULL.</param>
    /// <returns>The string read from the stream.</returns>
    internal string ReadString(int size)
    {
        if (_bitConverter == null)
        {
            throw new FileHandlerNotCorrectlyInitializedException();
        }
        
        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "String size cannot be less than 1.");
        }
        
        var array = new byte[size];
        Read(array);
        return _bitConverter.ToString(array);
    }
}