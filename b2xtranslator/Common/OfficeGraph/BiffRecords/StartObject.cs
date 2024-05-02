using System.Diagnostics;
using b2xtranslator.OfficeGraph.Structures;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.OfficeGraph.BiffRecords;

[OfficeGraphBiffRecord(GraphRecordNumber.StartObject)]
public class StartObject : OfficeGraphBiffRecord
{
    public const GraphRecordNumber ID = GraphRecordNumber.StartObject;
    public FrtHeaderOld frtHeaderOld;
    /// <summary>
    ///     An unsigned integer that specifies the object context.<br />
    ///     MUST be 0x0000.
    /// </summary>
    public ushort iObjectContext;
    /// <summary>
    ///     An unsigned integer that specifies additional information about the context of the object,
    ///     along with iObjectContext, iObjectInstance2 and iObjectKind.
    ///     This field MUST equal 0x0000 if iObjectKind equals 0x0010 or 0x0012.
    ///     MUST be a value from the following table if iObjectKind equals 0x0011:<br />
    ///     0x0008 = Specifies the application version. &lt;60&gt;<br />
    ///     0x0009 = Specifies the application version. &lt;61&gt;<br />
    ///     0x000A = Specifies the application version. &lt;62&gt;<br />
    ///     0x000B = Specifies the application version. &lt;63&gt;<br />
    ///     0x000C = Specifies the application version. &lt;64&gt;<br />
    /// </summary>
    public ushort iObjectInstance1;
    /// <summary>
    ///     An unsigned integer that specifies more information about the object context,
    ///     along with iObjectContext, iObjectInstance1 and iObjectKind. <br />
    ///     This field MUST equal 0x0000.
    /// </summary>
    public ushort iObjectInstance2;
    /// <summary>
    ///     An unsigned integer that specifies the type of object that is encompassed by the block.<br />
    ///     MUST be a value from the following table:<br />
    ///     0x0010<br />
    ///     0x0011<br />
    ///     0x0012
    /// </summary>
    public ushort iObjectKind;
    
    public StartObject(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        frtHeaderOld = new FrtHeaderOld(reader);
        iObjectKind = reader.ReadUInt16();
        iObjectContext = reader.ReadUInt16();
        iObjectInstance1 = reader.ReadUInt16();
        iObjectInstance2 = reader.ReadUInt16();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}