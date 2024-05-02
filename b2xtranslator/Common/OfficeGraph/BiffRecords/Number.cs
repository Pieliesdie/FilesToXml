using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.Number)]
public class Number : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.Number;
    /// <summary>
    ///     Column
    /// </summary>
    public ushort col;
    /// <summary>
    ///     Index to the XF Record
    /// </summary>
    public ushort ixfe;
    /// <summary>
    ///     The floating point number
    /// </summary>
    public double num;
    /// <summary>
    ///     Row
    /// </summary>
    public ushort rw;
    
    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="reader">Streamreader</param>
    /// <param name="id">Record ID - Recordtype</param>
    /// <param name="length">The recordlegth</param>
    public Number(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        rw = reader.ReadUInt16();
        col = reader.ReadUInt16();
        reader.ReadByte();
        ixfe = reader.ReadUInt16();
        num = reader.ReadDouble();
        
        // assert that the correct number of bytes has been read from the stream
        // Debug.Assert(this.Offset + this.Length == this.Reader.BaseStream.Position); 
    }
}