﻿using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.Structures;

/// <summary>
///     This structure specifies a future record.
/// </summary>
public class FrtHeader
{
    /// <summary>
    ///     A FrtFlags that specifies attributes for this record.
    ///     The value of grbitFrt.fFrtRef MUST be zero.
    /// </summary>
    public ushort grbitFrt;
    /// <summary>
    ///     An unsigned integer that specifies the record type identifier.
    ///     MUST be identical to the record type identifier of the containing record.
    /// </summary>
    public ushort rt;
    
    public FrtHeader(IStreamReader reader)
    {
        rt = reader.ReadUInt16();
        grbitFrt = reader.ReadUInt16();
        
        // ignore remaing record data
        reader.ReadBytes(8);
    }
}