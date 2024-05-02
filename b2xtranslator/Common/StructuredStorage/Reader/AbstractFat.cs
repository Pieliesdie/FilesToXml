using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Abstract class of a Fat in a compound file
///     Author: math
/// </summary>
internal abstract class AbstractFat
{
    protected int _addressesPerSector;
    protected InputHandler _fileHandler;
    protected Header _header;
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="header">Handle to the header of the compound file</param>
    /// <param name="fileHandler">Handle to the file handler of the compound file</param>
    internal AbstractFat(Header header, InputHandler fileHandler)
    {
        _header = header;
        _fileHandler = fileHandler;
        _addressesPerSector = _header.SectorSize / 4;
    }
    
    internal Stream _InternalFileStream => _fileHandler._InternalFileStream;
    internal abstract ushort SectorSize { get; }
    
    /// <summary>
    ///     Returns the sectors in a chain which starts at a given sector
    /// </summary>
    /// <param name="startSector">The start sector of the chain</param>
    /// <param name="maxCount">The maximum count of sectors in a chain</param>
    /// <param name="name">The name of a chain</param>
    internal List<uint> GetSectorChain(uint startSector, ulong maxCount, string name)
    {
        return GetSectorChain(startSector, maxCount, name, false);
    }
    
    /// <summary>
    ///     Returns the sectors in a chain which starts at a given sector
    /// </summary>
    /// <param name="startSector">The start sector of the chain</param>
    /// <param name="maxCount">The maximum count of sectors in a chain</param>
    /// <param name="name">The name of a chain</param>
    /// <param name="immediateCycleCheck">Flag whether to check for cycles in every loop</param>
    internal List<uint> GetSectorChain(uint startSector, ulong maxCount, string name, bool immediateCycleCheck)
    {
        var result = new List<uint> { startSector };
        
        while (true)
        {
            var nextSectorInStream = GetNextSectorInChain(result[result.Count - 1]);
            
            // Check for invalid sectors in chain
            if (nextSectorInStream == SectorId.DIFSECT || nextSectorInStream == SectorId.FATSECT || nextSectorInStream == SectorId.FREESECT)
            {
                throw new InvalidSectorInChainException();
            }
            
            if (nextSectorInStream == SectorId.ENDOFCHAIN)
            {
                break;
            }
            
            if (immediateCycleCheck)
            {
                if (result.Contains(nextSectorInStream))
                {
                    throw new ChainCycleDetectedException(name);
                }
            }
            
            result.Add(nextSectorInStream);
            
            // Chain too long
            if ((ulong)result.Count > maxCount)
            {
                throw new ChainSizeMismatchException(name);
            }
        }
        
        return result;
    }
    
    /// <summary>
    ///     Reads bytes into an array
    /// </summary>
    /// <param name="array">The array to read to</param>
    /// <param name="offset">The offset in the array to read to</param>
    /// <param name="count">The number of bytes to read</param>
    /// <returns>The number of bytes read</returns>
    internal int UncheckedRead(byte[] array, int offset, int count)
    {
        return _fileHandler.UncheckedRead(array, offset, count);
    }
    
    /// <summary>
    ///     Reads a byte at the current position of the file stream.
    ///     Advances the stream pointer accordingly.
    /// </summary>
    internal int UncheckedReadByte()
    {
        return _fileHandler.UncheckedReadByte();
    }
    
    /// <summary>
    ///     Returns the next sector in a chain
    /// </summary>
    /// <param name="currentSector">The current sector in the chain</param>
    /// <returns>The next sector in the chain</returns>
    protected abstract uint GetNextSectorInChain(uint currentSector);
    
    /// <summary>
    ///     Seeks to a given position in a sector
    /// </summary>
    /// <param name="sector">The sector to seek to</param>
    /// <param name="position">The position in the sector to seek to</param>
    /// <returns></returns>
    internal abstract long SeekToPositionInSector(long sector, long position);
}