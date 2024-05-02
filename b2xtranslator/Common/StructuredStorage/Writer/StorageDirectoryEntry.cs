using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Represents a storage directory entry in a structured storage.
///     Author: math
/// </summary>
public class StorageDirectoryEntry : BaseDirectoryEntry
{
    // The stream and storage directory entries of this storage directory entry
    private readonly List<BaseDirectoryEntry> _allDirectoryEntries = new();
    
    // The storage directory entries of this storage directory entry
    // The stream directory entries of this storage directory entry
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="name">The name of the directory entry.</param>
    /// <param name="context">The current context.</param>
    internal StorageDirectoryEntry(string name, StructuredStorageContext context)
        : base(name, context)
    {
        Type = DirectoryEntryType.STGTY_STORAGE;
    }
    
    internal List<StreamDirectoryEntry> StreamDirectoryEntries { get; } = new();
    internal List<StorageDirectoryEntry> StorageDirectoryEntries { get; } = new();
    
    /// <summary>
    ///     Adds a stream directory entry to this storage directory entry.
    /// </summary>
    /// <param name="name">The name of the stream directory entry to add.</param>
    /// <param name="stream">The stream referenced by the stream directory entry</param>
    public void AddStreamDirectoryEntry(string name, Stream stream)
    {
        if (StreamDirectoryEntries.Exists(delegate(StreamDirectoryEntry a) { return name == a.Name; }))
        {
            return;
        }
        
        var newDirEntry = new StreamDirectoryEntry(name, stream, Context);
        StreamDirectoryEntries.Add(newDirEntry);
        _allDirectoryEntries.Add(newDirEntry);
    }
    
    /// <summary>
    ///     Adds a storage directory entry to this storage directory entry.
    /// </summary>
    /// <param name="name">The name of the storage directory entry to add.</param>
    /// <returns>The storage directory entry whic hahs been added.</returns>
    public StorageDirectoryEntry AddStorageDirectoryEntry(string name)
    {
        StorageDirectoryEntry result = null;
        result = StorageDirectoryEntries.Find(delegate(StorageDirectoryEntry a) { return name == a.Name; });
        if (result != null)
        {
            // entry exists
            return result;
        }
        
        result = new StorageDirectoryEntry(name, Context);
        StorageDirectoryEntries.Add(result);
        _allDirectoryEntries.Add(result);
        return result;
    }
    
    /// <summary>
    ///     Sets the clsID.
    /// </summary>
    /// <param name="clsId">The clsId to set.</param>
    public void setClsId(Guid clsId)
    {
        ClsId = clsId;
    }
    
    /// <summary>
    ///     Recursively gets all storage directory entries starting at this directory entry.
    /// </summary>
    /// <returns>A list of directory entries.</returns>
    internal List<BaseDirectoryEntry> RecursiveGetAllDirectoryEntries()
    {
        var result = new List<BaseDirectoryEntry>();
        return RecursiveGetAllDirectoryEntries(result);
    }
    
    /// <summary>
    ///     The recursive implementation of the method RecursiveGetAllDirectoryEntries().
    /// </summary>
    private List<BaseDirectoryEntry> RecursiveGetAllDirectoryEntries(List<BaseDirectoryEntry> result)
    {
        foreach (var entry in StorageDirectoryEntries)
        {
            result.AddRange(entry.RecursiveGetAllDirectoryEntries());
        }
        
        foreach (var entry in StreamDirectoryEntries)
        {
            result.Add(entry);
        }
        
        if (!result.Contains(this))
        {
            result.Add(this);
        }
        
        return result;
    }
    
    /// <summary>
    ///     Creates the red-black-tree recursively
    /// </summary>
    internal void RecursiveCreateRedBlackTrees()
    {
        ChildSiblingSid = CreateRedBlackTree();
        
        foreach (var entry in StorageDirectoryEntries)
        {
            entry.RecursiveCreateRedBlackTrees();
        }
        
        //foreach (BaseDirectoryEntry entry in _allDirectoryEntries)
        //{
        //    UInt32 left = entry.LeftSiblingSid;
        //    UInt32 right = entry.RightSiblingSid;
        //    UInt32 child = entry.ChildSiblingSid;
        //    Console.WriteLine("{0:X02}: Left: {2:X02}, Right: {3:X02}, Child: {4:X02}, Name: {1}, Color: {5}", entry.Sid, entry.Name, (left > 0xFF) ? 0xFF : left, (right > 0xFF) ? 0xFF : right, (child > 0xFF) ? 0xFF : child, entry.Color.ToString());
        //}
        //Console.WriteLine("----------");
    }
    
    /// <summary>
    ///     Creates the red-black-tree for this directory entry
    /// </summary>
    private uint CreateRedBlackTree()
    {
        _allDirectoryEntries.Sort(DirectoryEntryComparison);
        
        foreach (var entry in _allDirectoryEntries)
        {
            entry.Sid = Context.getNewSid();
        }
        
        return setRelationsAndColorRecursive(_allDirectoryEntries, (int)Math.Floor(Math.Log(_allDirectoryEntries.Count, 2)), 0);
    }
    
    /// <summary>
    ///     Helper function for the method CreateRedBlackTree()
    /// </summary>
    /// <param name="entryList">The list of directory entries</param>
    /// <param name="treeHeight">The height of the balanced red-black-tree</param>
    /// <param name="treeLevel">The current tree level</param>
    /// <returns>The root of this red-black-tree</returns>
    private static uint setRelationsAndColorRecursive(List<BaseDirectoryEntry> entryList, int treeHeight, int treeLevel)
    {
        if (entryList.Count < 1)
        {
            return SectorId.FREESECT;
        }
        
        if (entryList.Count == 1)
        {
            if (treeLevel == treeHeight)
            {
                entryList[0].Color = DirectoryEntryColor.DE_RED;
            }
            
            return entryList[0].Sid;
        }
        
        var middleIndex = getMiddleIndex(entryList);
        var leftSubTree = entryList.GetRange(0, middleIndex);
        var rightSubTree = entryList.GetRange(middleIndex + 1, entryList.Count - middleIndex - 1);
        var leftmiddleIndex = getMiddleIndex(leftSubTree);
        var rightmiddleIndex = getMiddleIndex(rightSubTree);
        if (leftSubTree.Count > 0)
        {
            entryList[middleIndex].LeftSiblingSid = leftSubTree[leftmiddleIndex].Sid;
            setRelationsAndColorRecursive(leftSubTree, treeHeight, treeLevel + 1);
        }
        
        if (rightSubTree.Count > 0)
        {
            entryList[middleIndex].RightSiblingSid = rightSubTree[rightmiddleIndex].Sid;
            setRelationsAndColorRecursive(rightSubTree, treeHeight, treeLevel + 1);
        }
        
        return entryList[middleIndex].Sid;
    }
    
    /// <summary>
    ///     Calculation of the middle index of a list of directory entries.
    /// </summary>
    /// <param name="list">The input list.</param>
    /// <returns>The result</returns>
    private static int getMiddleIndex(List<BaseDirectoryEntry> list)
    {
        return (int)Math.Floor((list.Count - 1) / 2.0);
    }
    
    /// <summary>
    ///     Method for comparing directory entries (used in the red-black-tree).
    /// </summary>
    /// <param name="a">The 1st directory entry.</param>
    /// <param name="b">The 2nd directory entry.</param>
    /// <returns>Comparison result.</returns>
    protected int DirectoryEntryComparison(BaseDirectoryEntry a, BaseDirectoryEntry b)
    {
        if (a.Name.Length != b.Name.Length)
        {
            return a.Name.Length.CompareTo(b.Name.Length);
        }
        
        var aU = a.Name.ToUpper();
        var bU = b.Name.ToUpper();
        
        for (var i = 0; i < aU.Length; i++)
        {
            if (aU[i] != (uint)bU[i])
            {
                return ((uint)aU[i]).CompareTo(bU[i]);
            }
        }
        
        return 0;
    }
}