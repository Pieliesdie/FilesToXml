﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the end of the chart sheet substream in the workbook stream.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.EOF)]
public class EOF : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.EOF;
    
    public EOF(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // NOTE: This record is empty
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}