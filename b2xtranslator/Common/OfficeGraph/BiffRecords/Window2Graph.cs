﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies the visible portion of the datasheet. It specifies the
///     first row or first column to display when showing the datasheet.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.Window2Graph)]
public class Window2Graph : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Window2Graph;
    /// <summary>
    ///     A Graph_Col that specifies the first (top-left) visible column of the datasheet.
    ///     MUST be greater than or equal to 1.
    /// </summary>
    public ushort colFirst;
    /// <summary>
    ///     A Graph_Rw that specifies the first (top-left) visible row of the datasheet.
    ///     MUST be greater than or equal to 1.
    /// </summary>
    public ushort rowFirst;
    
    public Window2Graph(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        // ignore reserved bytes
        reader.ReadBytes(5);
        
        rowFirst = reader.ReadUInt16();
        colFirst = reader.ReadUInt16();
        
        // ignore reserved bytes
        reader.ReadBytes(5);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}