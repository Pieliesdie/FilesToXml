using System;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Common base class for stream and storage directory entries
///     Author: math
/// </summary>
public abstract class BaseDirectoryEntry : AbstractDirectoryEntry
{
    /// <summary>
    /// </summary>
    /// <param name="name">Name of the directory entry.</param>
    /// <param name="context">the current context</param>
    internal BaseDirectoryEntry(string name, StructuredStorageContext context)
    {
        Context = context;
        Name = name;
        setInitialValues();
    }
    
    internal StructuredStorageContext Context { get; }
    
    /// <summary>
    ///     Set the initial values
    /// </summary>
    private void setInitialValues()
    {
        ChildSiblingSid = SectorId.FREESECT;
        LeftSiblingSid = SectorId.FREESECT;
        RightSiblingSid = SectorId.FREESECT;
        ClsId = Guid.Empty;
        Color = DirectoryEntryColor.DE_BLACK;
        StartSector = 0x0;
        ClsId = Guid.Empty;
        UserFlags = 0x0;
        SizeOfStream = 0x0;
    }
    
    /// <summary>
    ///     Writes the directory entry to the directory stream of the current context
    /// </summary>
    internal void write()
    {
        var directoryStream = Context.DirectoryStream;
        var unicodeName = _name.ToCharArray();
        var paddingCounter = 0;
        foreach (ushort unicodeChar in unicodeName)
        {
            directoryStream.writeUInt16(unicodeChar);
            paddingCounter++;
        }
        
        while (paddingCounter < 32)
        {
            directoryStream.writeUInt16(0x0);
            paddingCounter++;
        }
        
        directoryStream.writeUInt16(LengthOfName);
        directoryStream.writeByte((byte)Type);
        directoryStream.writeByte((byte)Color);
        directoryStream.writeUInt32(LeftSiblingSid);
        directoryStream.writeUInt32(RightSiblingSid);
        directoryStream.writeUInt32(ChildSiblingSid);
        directoryStream.write(ClsId.ToByteArray());
        directoryStream.writeUInt32(UserFlags);
        //FILETIME set to 0x0
        directoryStream.write(new byte[16]);
        
        directoryStream.writeUInt32(StartSector);
        directoryStream.writeUInt64(SizeOfStream);
    }
    
    // Does nothing in the base class implementation.
    internal virtual void writeReferencedStream() { }
}