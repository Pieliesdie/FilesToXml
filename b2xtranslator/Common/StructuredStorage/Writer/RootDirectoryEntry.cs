using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which represents the root directory entry of a structured storage.
///     Author: math
/// </summary>
public class RootDirectoryEntry : StorageDirectoryEntry
{
    // The mini stream.
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="context">the current context</param>
    internal RootDirectoryEntry(StructuredStorageContext context)
        : base("Root Entry", context)
    {
        Type = DirectoryEntryType.STGTY_ROOT;
        Sid = 0x0;
    }
    
    internal OutputHandler MiniStream { get; } = new(new MemoryStream());
    
    /// <summary>
    ///     Writes the mini stream chain to the fat and the mini stream data to the output stream of the current context.
    /// </summary>
    internal override void writeReferencedStream()
    {
        var virtualMiniStream = new VirtualStream(MiniStream.BaseStream, Context.Fat, Context.Header.SectorSize, Context.TempOutputStream);
        virtualMiniStream.write();
        StartSector = virtualMiniStream.StartSector;
        SizeOfStream = virtualMiniStream.Length;
    }
}