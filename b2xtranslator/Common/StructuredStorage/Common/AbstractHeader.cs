using System;

namespace b2xtranslator.StructuredStorage.Common;

/// <summary>
///     Abstract class fo the header of a compound file.
///     Author: math
/// </summary>
internal abstract class AbstractHeader
{
    protected const ulong MAGIC_NUMBER = 0xE11AB1A1E011CFD0;
    
    // SectDifStart
    private uint _diFatStartSector;
    
    // SectDirStart
    private uint _directoryStartSector;
    protected AbstractIOHandler _ioHandler;
    
    // SectMiniFatStart
    private uint _miniFatStartSector;
    
    // UInt32ULMiniSectorCutoff
    private uint _miniSectorCutoff;
    
    // Minisector shift and Minisector size
    private ushort _miniSectorShift;
    
    // CSectDif
    private uint _noSectorsInDiFatChain;
    
    // CSectDir
    private uint _noSectorsInDirectoryChain4KB;
    
    // CSectFat
    private uint _noSectorsInFatChain;
    
    // CSectMiniFat
    private uint _noSectorsInMiniFatChain;
    
    // Sector shift and sector size
    private ushort _sectorShift;
    
    public ushort SectorShift
    {
        get => _sectorShift;
        set
        {
            _sectorShift = value;
            // Calculate sector size
            SectorSize = (ushort)Math.Pow(2, _sectorShift);
            if (_sectorShift != 9 && _sectorShift != 12)
            {
                throw new UnsupportedSizeException("SectorShift: " + _sectorShift);
            }
        }
    }
    
    public ushort SectorSize { get; private set; }
    
    public ushort MiniSectorShift
    {
        get => _miniSectorShift;
        set
        {
            _miniSectorShift = value;
            // Calculate mini sector size
            MiniSectorSize = (ushort)Math.Pow(2, _miniSectorShift);
            if (_miniSectorShift != 6)
            {
                throw new UnsupportedSizeException("MiniSectorShift: " + _miniSectorShift);
            }
        }
    }
    
    public ushort MiniSectorSize { get; private set; }
    
    public uint NoSectorsInDirectoryChain4KB
    {
        get => _noSectorsInDirectoryChain4KB;
        set
        {
            if (SectorSize == 512 && value != 0)
            {
                throw new ValueNotZeroException("_csectDir");
            }
            
            _noSectorsInDirectoryChain4KB = value;
        }
    }
    
    public uint NoSectorsInFatChain
    {
        get => _noSectorsInFatChain;
        set
        {
            _noSectorsInFatChain = value;
            if (value > _ioHandler.IOStreamSize / SectorSize)
            {
                throw new InvalidValueInHeaderException("NoSectorsInFatChain");
            }
        }
    }
    
    public uint DirectoryStartSector
    {
        get => _directoryStartSector;
        set
        {
            _directoryStartSector = value;
            if (value > _ioHandler.IOStreamSize / SectorSize && value != SectorId.ENDOFCHAIN)
            {
                throw new InvalidValueInHeaderException("DirectoryStartSector");
            }
        }
    }
    
    public uint MiniSectorCutoff
    {
        get => _miniSectorCutoff;
        set
        {
            _miniSectorCutoff = value;
            if (value != 0x1000)
            {
                throw new UnsupportedSizeException("MiniSectorCutoff");
            }
        }
    }
    
    public uint MiniFatStartSector
    {
        get => _miniFatStartSector;
        set
        {
            _miniFatStartSector = value;
            if (value > _ioHandler.IOStreamSize / SectorSize && value != SectorId.ENDOFCHAIN)
            {
                throw new InvalidValueInHeaderException("MiniFatStartSector");
            }
        }
    }
    
    public uint NoSectorsInMiniFatChain
    {
        get => _noSectorsInMiniFatChain;
        set
        {
            _noSectorsInMiniFatChain = value;
            if (value > _ioHandler.IOStreamSize / SectorSize)
            {
                throw new InvalidValueInHeaderException("NoSectorsInMiniFatChain");
            }
        }
    }
    
    public uint DiFatStartSector
    {
        get => _diFatStartSector;
        set
        {
            _diFatStartSector = value;
            if (value > _ioHandler.IOStreamSize / SectorSize && value != SectorId.ENDOFCHAIN && value != SectorId.FREESECT)
            {
                throw new InvalidValueInHeaderException("DiFatStartSector",
                    $"Details: value={value};_ioHandler.IOStreamSize={_ioHandler.IOStreamSize};SectorSize={SectorSize}; SectorId.ENDOFCHAIN: {SectorId.ENDOFCHAIN}");
            }
        }
    }
    
    public uint NoSectorsInDiFatChain
    {
        get => _noSectorsInDiFatChain;
        set
        {
            _noSectorsInDiFatChain = value;
            if (value > _ioHandler.IOStreamSize / SectorSize)
            {
                throw new InvalidValueInHeaderException("NoSectorsInDiFatChain");
            }
        }
    }
}