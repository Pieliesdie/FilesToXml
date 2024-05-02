using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which pools the different elements of a structured storage in a context.
///     Author math.
/// </summary>
internal class StructuredStorageContext
{
    // The handler of the directory stream of this context.
    
    // The fat of this context.
    
    // The header of this context.
    
    // The internal bit converter of this context.
    
    // The mini fat of this context.
    
    // The root directroy entry of this context.
    private uint _sidCounter;
    
    // The handler of the output stream of this context.
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    internal StructuredStorageContext()
    {
        TempOutputStream = new OutputHandler(new MemoryStream());
        DirectoryStream = new OutputHandler(new MemoryStream());
        Header = new Header(this);
        InternalBitConverter = new InternalBitConverter(true);
        Fat = new Fat(this);
        MiniFat = new MiniFat(this);
        RootDirectoryEntry = new RootDirectoryEntry(this);
    }
    
    internal Header Header { get; }
    internal Fat Fat { get; }
    internal MiniFat MiniFat { get; }
    internal OutputHandler TempOutputStream { get; }
    internal OutputHandler DirectoryStream { get; }
    internal InternalBitConverter InternalBitConverter { get; }
    public RootDirectoryEntry RootDirectoryEntry { get; }
    
    /// <summary>
    ///     Returns a new sid for directory entries in this context.
    /// </summary>
    /// <returns>The new sid.</returns>
    internal uint getNewSid()
    {
        return ++_sidCounter;
    }
}