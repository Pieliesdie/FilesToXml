using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

public class UnknownGraphRecord : OfficeGraphBiffRecord
{
    public byte[] Content;
    
    public UnknownGraphRecord(IStreamReader reader, ushort id, ushort length)
        : base(reader, (GraphRecordNumber)id, length)
    {
        Content = reader.ReadBytes(length);
    }
}