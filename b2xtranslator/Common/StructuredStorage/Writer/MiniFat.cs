using System;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Represents the minifat of a structured storage.
///     Author: math
/// </summary>
internal class MiniFat : AbstractFat
{
    // Start sector of the minifat.
    
    // Number of sectors in the mini fat.
    
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="context">the current context</param>
    internal MiniFat(StructuredStorageContext context)
        : base(context) { }
    
    internal uint MiniFatStart { get; private set; } = SectorId.FREESECT;
    internal uint NumMiniFatSectors { get; private set; }
    
    /// <summary>
    ///     Writes minifat chain to fat and writes the minifat data to the output stream of the current context.
    /// </summary>
    internal override void write()
    {
        NumMiniFatSectors = (uint)Math.Ceiling(_entries.Count * 4 / (double)_context.Header.SectorSize);
        MiniFatStart = _context.Fat.writeChain(NumMiniFatSectors);
        
        _context.TempOutputStream.writeSectors(_context.InternalBitConverter.getBytes(_entries).ToArray(), _context.Header.SectorSize, SectorId.FREESECT);
    }
}