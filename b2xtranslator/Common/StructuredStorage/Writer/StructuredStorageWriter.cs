using System.IO;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     The root class for creating a structured storage
///     Author: math
/// </summary>
public class StructuredStorageWriter
{
    private readonly StructuredStorageContext _context;
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    public StructuredStorageWriter()
    {
        _context = new StructuredStorageContext();
    }
    
    // The root directory entry of this structured storage.
    public StorageDirectoryEntry RootDirectoryEntry => _context.RootDirectoryEntry;
    
    /// <summary>
    ///     Writes the structured storage to a given stream.
    /// </summary>
    /// <param name="outputStream">The output stream.</param>
    public void write(Stream outputStream)
    {
        _context.RootDirectoryEntry.RecursiveCreateRedBlackTrees();
        
        var allEntries = _context.RootDirectoryEntry.RecursiveGetAllDirectoryEntries();
        allEntries.Sort(
            delegate(BaseDirectoryEntry a, BaseDirectoryEntry b)
            {
                return a.Sid.CompareTo(b.Sid);
            }
        );
        
        //foreach (BaseDirectoryEntry entry in allEntries)
        //{
        //    Console.WriteLine(entry.Sid + ":");
        //    Console.WriteLine("{0}: {1}", entry.Name, entry.LengthOfName);
        //    string hexName = "";
        //    string hexNameU = "";
        //    for (int i = 0; i < entry.Name.Length; i++)
        //    {
        //        hexName += String.Format("{0:X2} ", (UInt32)entry.Name[i]);
        //        hexNameU += String.Format("{0:X2} ", (UInt32)entry.Name.ToUpper()[i]);
        //    }
        //    Console.WriteLine("{0}", hexName);
        //    Console.WriteLine("{0}", hexNameU);
        
        //    UInt32 left = entry.LeftSiblingSid;
        //    UInt32 right = entry.RightSiblingSid;
        //    UInt32 child = entry.ChildSiblingSid;
        //    Console.WriteLine("{0:X02}: Left: {2:X02}, Right: {3:X02}, Child: {4:X02}, Name: {1}, Color: {5}", entry.Sid, entry.Name, (left > 0xFF) ? 0xFF : left, (right > 0xFF) ? 0xFF : right, (child > 0xFF) ? 0xFF : child, entry.Color.ToString());
        //    Console.WriteLine("----------");
        //    Console.WriteLine("");
        //}            
        
        // write Streams
        foreach (var entry in allEntries)
        {
            if (entry.Sid == 0x0)
            {
                // root entry
                continue;
            }
            
            entry.writeReferencedStream();
        }
        
        // root entry has to be written after all other streams as it contains the ministream to which other _entries write to
        _context.RootDirectoryEntry.writeReferencedStream();
        
        // write Directory Entries to directory stream
        foreach (var entry in allEntries)
        {
            entry.write();
        }
        
        // Directory Entry: 128 bytes            
        var dirEntriesPerSector = _context.Header.SectorSize / 128u;
        var numToPad = dirEntriesPerSector - (uint)allEntries.Count % dirEntriesPerSector;
        
        var emptyEntry = new EmptyDirectoryEntry(_context);
        for (var i = 0; i < numToPad; i++)
        {
            emptyEntry.write();
        }
        
        // write directory stream
        var virtualDirectoryStream = new VirtualStream(_context.DirectoryStream.BaseStream, _context.Fat, _context.Header.SectorSize, _context.TempOutputStream);
        virtualDirectoryStream.write();
        _context.Header.DirectoryStartSector = virtualDirectoryStream.StartSector;
        if (_context.Header.SectorSize == 0x1000)
        {
            _context.Header.NoSectorsInDirectoryChain4KB = virtualDirectoryStream.SectorCount;
        }
        
        // write MiniFat
        _context.MiniFat.write();
        _context.Header.MiniFatStartSector = _context.MiniFat.MiniFatStart;
        _context.Header.NoSectorsInMiniFatChain = _context.MiniFat.NumMiniFatSectors;
        
        // write fat
        _context.Fat.write();
        
        // set header values
        _context.Header.NoSectorsInDiFatChain = _context.Fat.NumDiFatSectors;
        _context.Header.NoSectorsInFatChain = _context.Fat.NumFatSectors;
        _context.Header.DiFatStartSector = _context.Fat.DiFatStartSector;
        
        // write header
        _context.Header.write();
        
        // write temporary streams to the output streams.
        _context.Header.writeToStream(outputStream);
        _context.TempOutputStream.writeToStream(outputStream);
    }
}