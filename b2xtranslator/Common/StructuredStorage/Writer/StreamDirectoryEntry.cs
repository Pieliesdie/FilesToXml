using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Represents a stream directory entry in a structured storage.
///     Author: math
/// </summary>
internal class StreamDirectoryEntry : BaseDirectoryEntry
{
    private readonly Stream _stream;
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="name">Name of the stream directory entry.</param>
    /// <param name="stream">The stream referenced by the stream directory entry.</param>
    /// <param name="context">The current context.</param>
    internal StreamDirectoryEntry(string name, Stream stream, StructuredStorageContext context)
        : base(name, context)
    {
        _stream = stream;
        Type = DirectoryEntryType.STGTY_STREAM;
    }
    
    /// <summary>
    ///     Writes the referenced stream chain to the fat and the referenced stream data to the output stream of the current
    ///     context.
    /// </summary>
    internal override void writeReferencedStream()
    {
        VirtualStream vStream = null;
        if (_stream.Length < Context.Header.MiniSectorCutoff)
        {
            vStream = new VirtualStream(_stream, Context.MiniFat, Context.Header.MiniSectorSize, Context.RootDirectoryEntry.MiniStream);
        }
        else
        {
            vStream = new VirtualStream(_stream, Context.Fat, Context.Header.SectorSize, Context.TempOutputStream);
        }
        
        vStream.write();
        StartSector = vStream.StartSector;
        SizeOfStream = vStream.Length;
    }
}