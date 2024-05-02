using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Represents the Fat in a compound file
///     Author: math
/// </summary>
internal class Fat : AbstractFat
{
    private readonly List<uint> _sectorsUsedByDiFat = new();
    private readonly List<uint> _sectorsUsedByFat = new();
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="header">Handle to the header of the compound file</param>
    /// <param name="fileHandler">Handle to the file handler of the compound file</param>
    internal Fat(Header header, InputHandler fileHandler)
        : base(header, fileHandler)
    {
        Init();
    }
    
    internal override ushort SectorSize => _header.SectorSize;
    
    /// <summary>
    ///     Seeks to a given position in a sector
    /// </summary>
    /// <param name="sector">The sector to seek to</param>
    /// <param name="position">The position in the sector to seek to</param>
    /// <returns>The new position in the stream.</returns>
    internal override long SeekToPositionInSector(long sector, long position)
    {
        return _fileHandler.SeekToPositionInSector(sector, position);
    }
    
    /// <summary>
    ///     Returns the next sector in a chain
    /// </summary>
    /// <param name="currentSector">The current sector in the chain</param>
    /// <returns>The next sector in the chain</returns>
    protected override uint GetNextSectorInChain(uint currentSector)
    {
        var sectorInFile = _sectorsUsedByFat[(int)(currentSector / _addressesPerSector)];
        // calculation of position:
        // currentSector % _addressesPerSector = number of address in the sector address
        // address uses 32 bit = 4 bytes
        _fileHandler.SeekToPositionInSector(sectorInFile, 4 * (currentSector % _addressesPerSector));
        return _fileHandler.ReadUInt32();
    }
    
    /// <summary>
    ///     Initalizes the Fat
    /// </summary>
    private void Init()
    {
        ReadFirst109SectorsUsedByFAT();
        ReadSectorsUsedByFatFromDiFat();
        CheckConsistency();
    }
    
    /// <summary>
    ///     Reads the first 109 sectors of the Fat stored in the header
    /// </summary>
    private void ReadFirst109SectorsUsedByFAT()
    {
        // Header sector: -1
        _fileHandler.SeekToPositionInSector(-1, 0x4C);
        uint fatSector;
        for (var i = 0; i < 109; i++)
        {
            fatSector = _fileHandler.ReadUInt32();
            if (fatSector == SectorId.FREESECT)
            {
                break;
            }
            
            _sectorsUsedByFat.Add(fatSector);
        }
    }
    
    /// <summary>
    ///     Reads the sectors of the Fat which are stored in the DiFat
    /// </summary>
    private void ReadSectorsUsedByFatFromDiFat()
    {
        if (_header.DiFatStartSector == SectorId.ENDOFCHAIN || _header.NoSectorsInDiFatChain == 0x0)
        {
            return;
        }
        
        _fileHandler.SeekToSector(_header.DiFatStartSector);
        var lastFatSectorFound = false;
        _sectorsUsedByDiFat.Add(_header.DiFatStartSector);
        
        while (true)
        {
            // Add all addresses contained in the current difat sector except the last address (it points to next difat sector)
            for (var i = 0; i < _addressesPerSector - 1; i++)
            {
                var fatSector = _fileHandler.ReadUInt32();
                if (fatSector == SectorId.FREESECT)
                {
                    lastFatSectorFound = true;
                    break;
                }
                
                _sectorsUsedByFat.Add(fatSector);
            }
            
            if (lastFatSectorFound)
            {
                break;
            }
            
            // Last address in difat sector points to next difat sector
            var nextDiFatSector = _fileHandler.ReadUInt32();
            if (nextDiFatSector == SectorId.FREESECT || nextDiFatSector == SectorId.ENDOFCHAIN)
            {
                break;
            }
            
            _sectorsUsedByDiFat.Add(nextDiFatSector);
            _fileHandler.SeekToSector(nextDiFatSector);
            
            if (_sectorsUsedByDiFat.Count > _header.NoSectorsInDiFatChain)
            {
                throw new ChainSizeMismatchException("DiFat");
            }
        }
    }
    
    /// <summary>
    ///     Checks whether the sizes specified in the header matches the actual sizes
    /// </summary>
    private void CheckConsistency()
    {
        if (_sectorsUsedByDiFat.Count != _header.NoSectorsInDiFatChain
            || _sectorsUsedByFat.Count != _header.NoSectorsInFatChain)
        {
            throw new ChainSizeMismatchException("Fat/DiFat");
        }
    }
}