using System;
using System.Collections.Generic;
using b2xtranslator.StructuredStorage.Common;

namespace b2xtranslator.StructuredStorage.Writer;

/// <summary>
///     Class which represents the fat of a structured storage.
///     Author: math
/// </summary>
internal class Fat : AbstractFat
{
    private readonly List<uint> _diFatEntries = new();
    
    // Start sector of the difat.
    
    // Number of sectors used by the difat.
    
    // Number of sectors used by the fat.
    
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="context">the current context</param>
    internal Fat(StructuredStorageContext context)
        : base(context) { }
    
    internal uint NumFatSectors { get; private set; }
    internal uint NumDiFatSectors { get; private set; }
    internal uint DiFatStartSector { get; private set; }
    
    /// <summary>
    ///     Writes the difat entries to the fat
    /// </summary>
    /// <param name="sectorCount">Number of difat sectors.</param>
    /// <returns>Start sector of the difat.</returns>
    private uint writeDiFatEntriesToFat(uint sectorCount)
    {
        if (sectorCount == 0)
        {
            return SectorId.ENDOFCHAIN;
        }
        
        var startSector = _currentEntry;
        
        for (var i = 0; i < sectorCount; i++)
        {
            _currentEntry++;
            _entries.Add(SectorId.DIFSECT);
        }
        
        return startSector;
    }
    
    /// <summary>
    ///     Writes the difat sectors to the output stream of the current context
    /// </summary>
    /// <param name="fatStartSector"></param>
    private void writeDiFatSectorsToStream(uint fatStartSector)
    {
        // Add all entries of the difat
        for (uint i = 0; i < NumFatSectors; i++)
        {
            _diFatEntries.Add(fatStartSector + i);
        }
        
        // Write the first 109 entries into the header
        for (var i = 0; i < 109; i++)
        {
            if (i < _diFatEntries.Count)
            {
                _context.Header.writeNextDiFatSector(_diFatEntries[i]);
            }
            else
            {
                _context.Header.writeNextDiFatSector(SectorId.FREESECT);
            }
        }
        
        if (_diFatEntries.Count <= 109)
        {
            return;
        }
        
        // handle remaining difat entries 
        
        var greaterDiFatEntries = new List<uint>();
        
        for (var i = 0; i < _diFatEntries.Count - 109; i++)
        {
            greaterDiFatEntries.Add(_diFatEntries[i + 109]);
        }
        
        var diFatLink = DiFatStartSector + 1;
        var addressesInSector = _context.Header.SectorSize / 4;
        var sectorSplit = addressesInSector;
        
        // split difat at sector boundary and add link to next difat sector
        while (greaterDiFatEntries.Count >= sectorSplit)
        {
            greaterDiFatEntries.Insert(sectorSplit - 1, diFatLink);
            diFatLink++;
            sectorSplit += addressesInSector;
        }
        
        // pad sector
        for (var i = greaterDiFatEntries.Count; i % (_context.Header.SectorSize / 4) != 0; i++)
        {
            greaterDiFatEntries.Add(SectorId.FREESECT);
        }
        
        greaterDiFatEntries.RemoveAt(greaterDiFatEntries.Count - 1);
        greaterDiFatEntries.Add(SectorId.ENDOFCHAIN);
        
        var output = _context.InternalBitConverter.getBytes(greaterDiFatEntries);
        
        // consistency check
        if (output.Count % _context.Header.SectorSize != 0)
        {
            throw new DiFatInconsistentException();
        }
        
        // write remaining difat sectors to stream
        _context.TempOutputStream.writeSectors(output.ToArray(), _context.Header.SectorSize, SectorId.FREESECT);
    }
    
    /// <summary>
    ///     Marks the difat and fat sectors in the fat and writes the difat and fat data to the output stream of the current
    ///     context.
    /// </summary>
    internal override void write()
    {
        // calculation of _numFatSectors and _numDiFatSectors (depending on each other)
        NumDiFatSectors = 0;
        while (true)
        {
            var numDiFatSectorsOld = NumDiFatSectors;
            NumFatSectors = (uint)Math.Ceiling(_entries.Count * 4 / (double)_context.Header.SectorSize) + NumDiFatSectors;
            NumDiFatSectors = NumFatSectors <= 109 ? 0 : (uint)Math.Ceiling((NumFatSectors - 109) * 4 / (double)(_context.Header.SectorSize - 1));
            if (numDiFatSectorsOld == NumDiFatSectors)
            {
                break;
            }
        }
        
        // writeDiFat
        DiFatStartSector = writeDiFatEntriesToFat(NumDiFatSectors);
        writeDiFatSectorsToStream(_currentEntry);
        
        // Denote Fat entries in Fat
        for (var i = 0; i < NumFatSectors; i++)
        {
            _entries.Add(SectorId.FATSECT);
        }
        
        // write Fat
        _context.TempOutputStream.writeSectors(_context.InternalBitConverter.getBytes(_entries).ToArray(), _context.Header.SectorSize, SectorId.FREESECT);
    }
}