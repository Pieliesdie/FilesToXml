﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record is unused.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.MaxStatus)]
public class MaxStatus : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.MaxStatus;
    
    public MaxStatus(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        reader.ReadBytes(2);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}