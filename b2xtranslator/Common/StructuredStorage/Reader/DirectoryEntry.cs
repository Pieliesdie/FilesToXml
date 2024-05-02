using System;
using b2xtranslator.StructuredStorage.Common;
using b2xtranslator.Tools;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Encapsulates a directory entry
///     Author: math
/// </summary>
public class DirectoryEntry : AbstractDirectoryEntry
{
    private readonly InputHandler _fileHandler;
    private readonly Header _header;
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="header">Handle to the header of the compound file</param>
    /// <param name="fileHandler">Handle to the file handler of the compound file</param>
    /// <param name="sid">The sid of the directory entry</param>
    internal DirectoryEntry(Header header, InputHandler fileHandler, uint sid, string path) : base(sid)
    {
        _header = header;
        _fileHandler = fileHandler;
        //_sid = sid;            
        ReadDirectoryEntry();
        _path = path;
    }
    
    /// <summary>
    ///     Reads the values of the directory entry. The position of the file handler must be at the start of a directory
    ///     entry.
    /// </summary>
    private void ReadDirectoryEntry()
    {
        Name = _fileHandler.ReadString(64);
        
        // Name length check: lengthOfName = length of the element in bytes including Unicode NULL
        var lengthOfName = _fileHandler.ReadUInt16();
        // Commented out due to trouble with odd unicode-named streams in PowerPoint -- flgr
        /*if (lengthOfName != (_name.Length + 1) * 2)
        {
            throw new InvalidValueInDirectoryEntryException("_cb");
        }*/
        // Added warning - math
        if (lengthOfName != (_name.Length + 1) * 2)
        {
            TraceLogger.Warning("Length of the name (_cb) of stream '" + Name + "' is not correct.");
        }
        
        Type = (DirectoryEntryType)_fileHandler.ReadByte();
        Color = (DirectoryEntryColor)_fileHandler.ReadByte();
        LeftSiblingSid = _fileHandler.ReadUInt32();
        RightSiblingSid = _fileHandler.ReadUInt32();
        ChildSiblingSid = _fileHandler.ReadUInt32();
        
        var array = new byte[16];
        _fileHandler.Read(array);
        ClsId = new Guid(array);
        
        UserFlags = _fileHandler.ReadUInt32();
        // Omit creation time
        _fileHandler.ReadUInt64();
        // Omit modification time 
        _fileHandler.ReadUInt64();
        StartSector = _fileHandler.ReadUInt32();
        
        var sizeLow = _fileHandler.ReadUInt32();
        var sizeHigh = _fileHandler.ReadUInt32();
        
        if (_header.SectorSize == 512 && sizeHigh != 0x0)
        {
            // Must be zero according to the specification. However, this requirement can be ommited.
            TraceLogger.Warning("ul_SizeHigh of stream '" + Name + "' should be zero as sector size is 512.");
            sizeHigh = 0x0;
        }
        
        SizeOfStream = ((ulong)sizeHigh << 32) + sizeLow;
    }
}