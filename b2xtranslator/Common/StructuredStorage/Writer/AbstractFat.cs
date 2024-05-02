using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Abstract class of a Fat in a compound file
///     Author: math
/// </summary>
internal abstract class AbstractFat
{
    protected StructuredStorageContext _context;
    protected uint _currentEntry;
    protected List<uint> _entries = new();
    
    /// <summary>
    ///     Constructor
    ///     <param name="context">the current context</param>
    /// </summary>
    protected AbstractFat(StructuredStorageContext context)
    {
        _context = context;
    }
    
    /// <summary>
    ///     Write a chain to the fat.
    /// </summary>
    /// <param name="entryCount">number of entries in the chain</param>
    /// <returns></returns>
    internal uint writeChain(uint entryCount)
    {
        if (entryCount == 0)
        {
            return SectorId.FREESECT;
        }
        
        var startSector = _currentEntry;
        
        for (var i = 0; i < entryCount - 1; i++)
        {
            _currentEntry++;
            _entries.Add(_currentEntry);
        }
        
        _currentEntry++;
        _entries.Add(SectorId.ENDOFCHAIN);
        
        return startSector;
    }
    
    internal abstract void write();
}