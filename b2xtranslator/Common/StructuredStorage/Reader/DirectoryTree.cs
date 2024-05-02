using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Represents the directory structure of a compound file
///     Author: math
/// </summary>
internal class DirectoryTree
{
    private readonly List<DirectoryEntry> _directoryEntries = new();
    private readonly Fat _fat;
    private readonly InputHandler _fileHandler;
    private readonly Header _header;
    private List<uint> _sectorsUsedByDirectory;
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="fat">Handle to the Fat of the compound file</param>
    /// <param name="header">Handle to the header of the compound file</param>
    /// <param name="fileHandler">Handle to the file handler of the compound file</param>
    internal DirectoryTree(Fat fat, Header header, InputHandler fileHandler)
    {
        _fat = fat;
        _header = header;
        _fileHandler = fileHandler;
        Init(_header.DirectoryStartSector);
    }
    
    /// <summary>
    ///     Inits the directory
    /// </summary>
    /// <param name="startSector">The sector containing the root of the directory</param>
    private void Init(uint startSector)
    {
        if (_header.NoSectorsInDirectoryChain4KB > 0)
        {
            _sectorsUsedByDirectory = _fat.GetSectorChain(startSector, _header.NoSectorsInDirectoryChain4KB, "Directory");
        }
        else
        {
            _sectorsUsedByDirectory = _fat.GetSectorChain(startSector, (ulong)Math.Ceiling((double)_fileHandler.IOStreamSize / _header.SectorSize), "Directory", true);
        }
        
        GetAllDirectoryEntriesRecursive(0, "");
    }
    
    /// <summary>
    ///     Determines the directory _entries in a compound file recursively
    /// </summary>
    /// <param name="sid">start sid</param>
    private void GetAllDirectoryEntriesRecursive(uint sid, string path)
    {
        var entry = ReadDirectoryEntry(sid, path);
        var left = entry.LeftSiblingSid;
        var right = entry.RightSiblingSid;
        var child = entry.ChildSiblingSid;
        //Console.WriteLine("{0:X02}: Left: {2:X02}, Right: {3:X02}, Child: {4:X02}, Name: {1}, Color: {5}", entry.Sid, entry.Name, (left > 0xFF)? 0xFF : left, (right > 0xFF)? 0xFF : right, (child > 0xFF)? 0xFF : child, entry.Color.ToString() );
        
        // Check for cycle
        if (_directoryEntries.Exists(delegate(DirectoryEntry x) { return x.Sid == entry.Sid; }))
        {
            throw new ChainCycleDetectedException("DirectoryEntries");
        }
        
        _directoryEntries.Add(entry);
        
        // Left sibling
        if (left != SectorId.NOSTREAM)
        {
            GetAllDirectoryEntriesRecursive(left, path);
        }
        
        // Right sibling
        if (right != SectorId.NOSTREAM)
        {
            GetAllDirectoryEntriesRecursive(right, path);
        }
        
        // Child
        if (child != SectorId.NOSTREAM)
        {
            GetAllDirectoryEntriesRecursive(child, path + (sid == 0 ? "" : entry.Name) + "\\");
        }
    }
    
    /// <summary>
    ///     Returns a directory entry for a given sid
    /// </summary>
    private DirectoryEntry ReadDirectoryEntry(uint sid, string path)
    {
        SeekToDirectoryEntry(sid);
        var result = new DirectoryEntry(_header, _fileHandler, sid, path);
        return result;
    }
    
    /// <summary>
    ///     Seeks to the start sector of the directory entry of the given sid
    /// </summary>
    private void SeekToDirectoryEntry(uint sid)
    {
        var sectorInDirectoryChain = (int)(sid * Measures.DirectoryEntrySize) / _header.SectorSize;
        if (sectorInDirectoryChain < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        
        _fileHandler.SeekToPositionInSector(_sectorsUsedByDirectory[sectorInDirectoryChain], sid * Measures.DirectoryEntrySize % _header.SectorSize);
    }
    
    /// <summary>
    ///     Returns the directory entry with the given name/path
    /// </summary>
    internal DirectoryEntry GetDirectoryEntry(string path)
    {
        if (path.Length < 1)
        {
            return null;
        }
        
        if (path[0] == '\\')
        {
            return _directoryEntries.Find(delegate(DirectoryEntry entry) { return entry.Path == path; });
        }
        
        return _directoryEntries.Find(delegate(DirectoryEntry entry) { return entry.Name == path; });
    }
    
    /// <summary>
    ///     Returns the directory entry with the given sid
    /// </summary>
    internal DirectoryEntry GetDirectoryEntry(uint sid)
    {
        return _directoryEntries.Find(delegate(DirectoryEntry entry) { return entry.Sid == sid; });
    }
    
    /// <summary>
    ///     Returns the start sector of the mini stream
    /// </summary>
    internal uint GetMiniStreamStart()
    {
        var root = GetDirectoryEntry(0);
        if (root == null)
        {
            throw new StreamNotFoundException("Root Entry");
        }
        
        return root.StartSector;
    }
    
    /// <summary>
    ///     Returns the size of the mini stream
    /// </summary>
    internal ulong GetSizeOfMiniStream()
    {
        var root = GetDirectoryEntry(0);
        if (root == null)
        {
            throw new StreamNotFoundException("Root Entry");
        }
        
        return root.SizeOfStream;
    }
    
    /// <summary>
    ///     Returns all entry names contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<string> GetNamesOfAllEntries()
    {
        var result = new List<string>();
        
        foreach (var entry in _directoryEntries)
        {
            result.Add(entry.Name);
        }
        
        return new ReadOnlyCollection<string>(result);
    }
    
    /// <summary>
    ///     Returns all entry paths contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<string> GetPathsOfAllEntries()
    {
        var result = new List<string>();
        
        foreach (var entry in _directoryEntries)
        {
            result.Add(entry.Path);
        }
        
        return new ReadOnlyCollection<string>(result);
    }
    
    /// <summary>
    ///     Returns all stream entry names contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<string> GetNamesOfAllStreamEntries()
    {
        var result = new List<string>();
        
        foreach (var entry in _directoryEntries)
        {
            if (entry.Type == DirectoryEntryType.STGTY_STREAM)
            {
                result.Add(entry.Name);
            }
        }
        
        return new ReadOnlyCollection<string>(result);
    }
    
    /// <summary>
    ///     Returns all stream entry paths contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<string> GetPathsOfAllStreamEntries()
    {
        var result = new List<string>();
        
        foreach (var entry in _directoryEntries)
        {
            if (entry.Type == DirectoryEntryType.STGTY_STREAM)
            {
                result.Add(entry.Path);
            }
        }
        
        return new ReadOnlyCollection<string>(result);
    }
    
    /// <summary>
    ///     Returns all _entries contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<DirectoryEntry> GetAllEntries()
    {
        return new ReadOnlyCollection<DirectoryEntry>(_directoryEntries);
    }
    
    /// <summary>
    ///     Returns all stream _entries contained in a compound file
    /// </summary>
    internal ReadOnlyCollection<DirectoryEntry> GetAllStreamEntries()
    {
        return new ReadOnlyCollection<DirectoryEntry>(_directoryEntries.FindAll(
            delegate(DirectoryEntry entry) { return entry.Type == DirectoryEntryType.STGTY_STREAM; }
        ));
    }
}