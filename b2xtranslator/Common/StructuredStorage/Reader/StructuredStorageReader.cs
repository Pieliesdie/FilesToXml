using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

[assembly: CLSCompliant(false)]

namespace b2xtranslator.StructuredStorage.Reader;

/// <summary>
///     Provides methods for accessing a compound file.
///     Author: math
/// </summary>
public sealed class StructuredStorageReader :
    IStructuredStorageReader
{
    private readonly DirectoryTree _directory;
    private readonly Fat _fat;
    private readonly InputHandler _fileHandler;
    private readonly Header _header;
    private readonly MiniFat _miniFat;
    
    /// <summary>Initalizes a handle to a compound file based on a stream</summary>
    /// <param name="stream">The stream to the storage</param>
    public StructuredStorageReader(Stream stream)
    {
        try
        {
            _fileHandler = new InputHandler(stream);
            _header = new Header(_fileHandler);
            _fat = new Fat(_header, _fileHandler);
            _directory = new DirectoryTree(_fat, _header, _fileHandler);
            _miniFat = new MiniFat(_fat, _header, _fileHandler, _directory.GetMiniStreamStart(), _directory.GetSizeOfMiniStream());
        }
        catch
        {
            Close();
            throw;
        }
    }
    
    /// <summary>Initalizes a handle to a compound file with the given name</summary>
    /// <param name="fileName">The name of the file including its path</param>
    public StructuredStorageReader(string fileName)
    {
        try
        {
            _fileHandler = new InputHandler(fileName);
            _header = new Header(_fileHandler);
            _fat = new Fat(_header, _fileHandler);
            _directory = new DirectoryTree(_fat, _header, _fileHandler);
            _miniFat = new MiniFat(_fat, _header, _fileHandler, _directory.GetMiniStreamStart(), _directory.GetSizeOfMiniStream());
        }
        catch
        {
            Close();
            throw;
        }
    }
    
    /// <summary>Get a collection of all entry names contained in a compound file</summary>
    public ICollection<string> FullNameOfAllEntries => _directory.GetPathsOfAllEntries();
    
    /// <summary>Get a collection of all stream entry names contained in a compound file</summary>
    public ICollection<string> FullNameOfAllStreamEntries => _directory.GetPathsOfAllStreamEntries();
    
    /// <summary>Get a collection of all _entries contained in a compound file</summary>
    public ICollection<DirectoryEntry> AllEntries => _directory.GetAllEntries();
    
    /// <summary>Get a collection of all stream _entries contained in a compound file</summary>
    public ICollection<DirectoryEntry> AllStreamEntries => _directory.GetAllStreamEntries();
    
    /// <summary>Get a handle to the RootDirectoryEntry.</summary>
    public DirectoryEntry RootDirectoryEntry => _directory.GetDirectoryEntry(0x0);
    
    /// <summary>
    ///     Returns a handle to a stream with the given name/path.
    ///     If a path is used, it must be preceeded by '\'.
    ///     The characters '\' ( if not separators in the path) and '%' must be masked by '%XXXX'
    ///     where 'XXXX' is the unicode in hex of '\' and '%', respectively
    /// </summary>
    /// <param name="path">The path of the virtual stream.</param>
    /// <returns>An object which enables access to the virtual stream.</returns>
    public VirtualStream GetStream(string path)
    {
        var entry = _directory.GetDirectoryEntry(path);
        if (entry == null)
        {
            throw new StreamNotFoundException(path);
        }
        
        if (entry.Type != DirectoryEntryType.STGTY_STREAM)
        {
            throw new WrongDirectoryEntryTypeException();
        }
        
        // only streams up to long.MaxValue are supported
        if (entry.SizeOfStream > long.MaxValue)
        {
            throw new UnsupportedSizeException(entry.SizeOfStream.ToString());
        }
        
        // Determine whether this stream is a "normal stream" or a stream in the mini stream
        if (entry.SizeOfStream < _header.MiniSectorCutoff)
        {
            return new VirtualStream(_miniFat, entry.StartSector, (long)entry.SizeOfStream, path);
        }
        
        return new VirtualStream(_fat, entry.StartSector, (long)entry.SizeOfStream, path);
    }
    
    /// <summary>Closes the file handle</summary>
    public void Close()
    {
        _fileHandler?.CloseStream();
    }
    
    public void Dispose()
    {
        Close();
    }
    
    /// <summary>
    ///     Returns a handle to a directory entry with the given name/path.
    ///     If a path is used, it must be preceeded by '\'.
    ///     The characters '\' ( if not separators in the path) and '%' must be masked by '%XXXX'
    ///     where 'XXXX' is the unicode in hex of '\' and '%', respectively
    /// </summary>
    /// <param name="path">The path of the directory entry.</param>
    /// <returns>An object which enables access to the directory entry.</returns>
    public DirectoryEntry GetEntry(string path)
    {
        var entry = _directory.GetDirectoryEntry(path);
        if (entry == null)
        {
            throw new DirectoryEntryNotFoundException(path);
        }
        
        return entry;
    }
}