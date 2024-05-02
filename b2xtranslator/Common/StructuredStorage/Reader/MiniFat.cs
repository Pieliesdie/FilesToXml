using System;
using System.Collections.Generic;
using System.Diagnostics;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Represents the MiniFat in a compound file
///     Author: math
/// </summary>
internal class MiniFat : AbstractFat
{
    private readonly Fat _fat;
    private readonly uint _miniStreamStart;
    private List<uint> _sectorsUsedByMiniFat = new();
    private List<uint> _sectorsUsedByMiniStream = new();
    private readonly ulong _sizeOfMiniStream;
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="fat">Handle to the Fat of the compound file</param>
    /// <param name="header">Handle to the header of the compound file</param>
    /// <param name="fileHandler">Handle to the file handler of the compound file</param>
    /// <param name="miniStreamStart">Address of the sector where the mini stream starts</param>
    internal MiniFat(Fat fat, Header header, InputHandler fileHandler, uint miniStreamStart, ulong sizeOfMiniStream)
        : base(header, fileHandler)
    {
        _fat = fat;
        _miniStreamStart = miniStreamStart;
        _sizeOfMiniStream = sizeOfMiniStream;
        Init();
    }
    
    internal override ushort SectorSize => _header.MiniSectorSize;
    
    /// <summary>
    ///     Seeks to a given position in a sector of the mini stream
    /// </summary>
    /// <param name="sector">The sector to seek to</param>
    /// <param name="position">The position in the sector to seek to</param>
    /// <returns>The new position in the stream.</returns>
    internal override long SeekToPositionInSector(long sector, long position)
    {
        var sectorInMiniStreamChain = (int)(sector * _header.MiniSectorSize / _fat.SectorSize);
        var offsetInSector = (int)(sector * _header.MiniSectorSize % _fat.SectorSize);
        
        if (position < 0)
        {
            throw new ArgumentOutOfRangeException("position");
        }
        
        return _fileHandler.SeekToPositionInSector(_sectorsUsedByMiniStream[sectorInMiniStreamChain], offsetInSector + position);
    }
    
    /// <summary>
    ///     Returns the next sector in a chain
    /// </summary>
    /// <param name="currentSector">The current sector in the chain</param>
    /// <returns>The next sector in the chain</returns>
    protected override uint GetNextSectorInChain(uint currentSector)
    {
        var sectorInFile = _sectorsUsedByMiniFat[(int)(currentSector / _addressesPerSector)];
        // calculation of position:
        // currentSector % _addressesPerSector = number of address in the sector address
        // address uses 32 bit = 4 bytes
        _fileHandler.SeekToPositionInSector(sectorInFile, 4 * ((int)currentSector % _addressesPerSector));
        return _fileHandler.ReadUInt32();
    }
    
    /// <summary>
    ///     Initalizes the Fat
    /// </summary>
    private void Init()
    {
        ReadSectorsUsedByMiniFAT();
        ReadSectorsUsedByMiniStream();
        CheckConsistency();
    }
    
    /// <summary>
    ///     Reads the sectors used by the MiniFat
    /// </summary>
    private void ReadSectorsUsedByMiniFAT()
    {
        if (_header.MiniFatStartSector == SectorId.ENDOFCHAIN || _header.NoSectorsInMiniFatChain == 0x0)
        {
            return;
        }
        
        _sectorsUsedByMiniFat = _fat.GetSectorChain(_header.MiniFatStartSector, _header.NoSectorsInMiniFatChain, "MiniFat");
    }
    
    /// <summary>
    ///     Reads the sectors used by the MiniFat
    /// </summary>
    private void ReadSectorsUsedByMiniStream()
    {
        if (_miniStreamStart == SectorId.ENDOFCHAIN)
        {
            return;
        }
        
        _sectorsUsedByMiniStream = _fat.GetSectorChain(_miniStreamStart, (ulong)Math.Ceiling((double)_sizeOfMiniStream / _header.SectorSize), "MiniStream");
    }
    
    /// <summary>
    ///     Checks whether the size specified in the header matches the actual size
    /// </summary>
    private void CheckConsistency()
    {
        if (_sectorsUsedByMiniFat.Count != _header.NoSectorsInMiniFatChain)
        {
            throw new ChainSizeMismatchException("MiniFat");
        }
        
        if (_sectorsUsedByMiniStream.Count != Math.Ceiling((double)_sizeOfMiniStream / _header.SectorSize))
        {
            Trace.TraceWarning("StructuredStorage: The number of sectors used by MiniFat does not match the specified size.");
            Trace.TraceInformation("StructuredStorage: _sectorsUsedByMiniStream.Count={0};_sizeOfMiniStream={1};_header.SectorSize={2}; Math.Ceiling={3}", _sectorsUsedByMiniStream.Count, _sizeOfMiniStream, _header.SectorSize,
                Math.Ceiling((double)_sizeOfMiniStream / _header.SectorSize));
            //throw new ChainSizeMismatchException("MiniStream");
        }
    }
}