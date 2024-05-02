using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which represents the header of a structured storage.
///     Author: math
/// </summary>
internal class Header : AbstractHeader
{
    private readonly StructuredStorageContext _context;
    private int _diFatSectorCount;
    private readonly List<byte> _diFatSectors = new();
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="context">the current context</param>
    internal Header(StructuredStorageContext context)
    {
        _ioHandler = new OutputHandler(new MemoryStream());
        _ioHandler.SetHeaderReference(this);
        _ioHandler.InitBitConverter(true);
        _context = context;
        setHeaderDefaults();
    }
    
    /// <summary>
    ///     Initializes header defaults.
    /// </summary>
    private void setHeaderDefaults()
    {
        MiniSectorShift = 6;
        SectorShift = 9;
        NoSectorsInDirectoryChain4KB = 0;
        MiniSectorCutoff = 4096;
    }
    
    /// <summary>
    ///     Writes the next difat sector (which is one of the first 109) to the header.
    /// </summary>
    /// <param name="sector"></param>
    internal void writeNextDiFatSector(uint sector)
    {
        if (_diFatSectorCount >= 109)
        {
            throw new DiFatInconsistentException();
        }
        
        _diFatSectors.AddRange(_context.InternalBitConverter.getBytes(sector));
        
        _diFatSectorCount++;
    }
    
    /// <summary>
    ///     Writes the header to the internal stream.
    /// </summary>
    internal void write()
    {
        var outputHandler = (OutputHandler)_ioHandler;
        
        // Magic number
        outputHandler.write(BitConverter.GetBytes(MAGIC_NUMBER));
        
        // CLSID
        outputHandler.write(new byte[16]);
        
        // Minor version
        outputHandler.writeUInt16(0x3E);
        
        // Major version: 512 KB sectors
        outputHandler.writeUInt16(0x03);
        
        // Byte ordering: little Endian
        outputHandler.writeUInt16(0xFFFE);
        
        outputHandler.writeUInt16(SectorShift);
        outputHandler.writeUInt16(MiniSectorShift);
        
        // reserved
        outputHandler.writeUInt16(0x0);
        outputHandler.writeUInt32(0x0);
        
        // cSectDir: 0x0 for 512 KB 
        outputHandler.writeUInt32(NoSectorsInDirectoryChain4KB);
        
        outputHandler.writeUInt32(NoSectorsInFatChain);
        outputHandler.writeUInt32(DirectoryStartSector);
        
        // reserved
        outputHandler.writeUInt32(0x0);
        
        outputHandler.writeUInt32(MiniSectorCutoff);
        outputHandler.writeUInt32(MiniFatStartSector);
        outputHandler.writeUInt32(NoSectorsInMiniFatChain);
        outputHandler.writeUInt32(DiFatStartSector);
        outputHandler.writeUInt32(NoSectorsInDiFatChain);
        
        // First 109 FAT Sectors
        outputHandler.write(_diFatSectors.ToArray());
        
        // Pad the rest
        if (SectorSize == 4096)
        {
            outputHandler.write(new byte[4096 - 512]);
        }
    }
    
    /// <summary>
    ///     Writes the internal header stream to the given stream.
    /// </summary>
    /// <param name="stream">The stream to which is written to.</param>
    internal void writeToStream(Stream stream)
    {
        var outputHandler = (OutputHandler)_ioHandler;
        outputHandler.writeToStream(stream);
    }
}