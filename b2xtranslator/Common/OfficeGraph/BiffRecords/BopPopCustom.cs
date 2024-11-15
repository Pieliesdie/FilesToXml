﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.BopPopCustom)]
public class BopPopCustom : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.BopPopCustom;
    
    public BopPopCustom(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        // TODO: place code here
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}