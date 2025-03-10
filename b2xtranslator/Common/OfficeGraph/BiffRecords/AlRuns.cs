﻿using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies rich text formatting within chart titles, trendline, and data labels.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.AlRuns)]
public class AlRuns : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.AlRuns;
    /// <summary>
    ///     An unsigned integer that specifies the number of rich text runs.
    ///     MUST be greater than or equal to 3 and less than or equal to 256.
    /// </summary>
    public ushort cRuns;
    public FormatRun[] rgRuns;
    
    public AlRuns(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        cRuns = reader.ReadUInt16();
        
        if (cRuns > 0)
        {
            rgRuns = new FormatRun[cRuns];
            
            for (var i = 0; i < cRuns; i++)
            {
                rgRuns[i] = new FormatRun(reader);
            }
        }
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}