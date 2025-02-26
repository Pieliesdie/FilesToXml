﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record is written but unused.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.CrtLink)]
public class CrtLink : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.CrtLink;
    
    public CrtLink(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        // This record is written but unused.
        reader.ReadBytes(10);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}