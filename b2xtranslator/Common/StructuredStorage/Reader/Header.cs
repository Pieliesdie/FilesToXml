using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Encapsulates the header of a compound file
///     Author: math
/// </summary>
internal class Header : AbstractHeader
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="fileHandler">The Handle to the file handler of the compound file</param>
    internal Header(InputHandler fileHandler)
    {
        _ioHandler = fileHandler;
        _ioHandler.SetHeaderReference(this);
        ReadHeader();
    }
    
    /// <summary>
    ///     Reads the header from the file stream
    /// </summary>
    private void ReadHeader()
    {
        var fileHandler = (InputHandler)_ioHandler;
        
        // Determine endian
        var byteArray16 = new byte[2];
        fileHandler.ReadPosition(byteArray16, 0x1C);
        if (byteArray16[0] == 0xFF && byteArray16[1] == 0xFE)
        {
            fileHandler.InitBitConverter(false);
        }
        else
        {
            // default little endian
            fileHandler.InitBitConverter(true);
        }
        
        var magicNumber = fileHandler.ReadUInt64(0x0);
        // Check for Magic Number                       
        if (magicNumber != MAGIC_NUMBER)
        {
            throw new MagicNumberException($"Found: {magicNumber,10:X}");
        }
        
        SectorShift = fileHandler.ReadUInt16(0x1E);
        MiniSectorShift = fileHandler.ReadUInt16();
        
        NoSectorsInDirectoryChain4KB = fileHandler.ReadUInt32(0x28);
        NoSectorsInFatChain = fileHandler.ReadUInt32();
        DirectoryStartSector = fileHandler.ReadUInt32();
        
        MiniSectorCutoff = fileHandler.ReadUInt32(0x38);
        MiniFatStartSector = fileHandler.ReadUInt32();
        NoSectorsInMiniFatChain = fileHandler.ReadUInt32();
        DiFatStartSector = fileHandler.ReadUInt32();
        NoSectorsInDiFatChain = fileHandler.ReadUInt32();
    }
}