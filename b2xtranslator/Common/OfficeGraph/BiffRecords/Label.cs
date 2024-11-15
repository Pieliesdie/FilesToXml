﻿using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.Label)]
public class Label : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Label;
    /// <summary>
    ///     An unsigned integer that specifies a zero-based index of a column in the datasheet that contains this structure.
    /// </summary>
    public ushort col;
    /// <summary>
    ///     An unsigned integer that specifies the identifier of a number format.
    ///     The identifier specified by this field MUST be a valid built-in number format identifier
    ///     or the identifier of a custom number format as specified using a Format record.
    /// </summary>
    public ushort ifmt;
    /// <summary>
    ///     An unsigned integer that specifies a zero-based index of a row in the datasheet that contains this structure. MUST
    ///     be less than or equal to 0x0F9F.
    /// </summary>
    public ushort rw;
    /// <summary>
    ///     A string that contains the string constant.
    /// </summary>
    public string stLabel;
    
    public Label(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        rw = reader.ReadUInt16();
        col = reader.ReadUInt16();
        reader.ReadByte(); // reserved
        ifmt = reader.ReadUInt16();
        stLabel = Utils.ReadShortXlUnicodeString(reader.BaseStream);
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}