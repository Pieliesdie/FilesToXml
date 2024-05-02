using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies properties of an error bar.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.SerAuxErrBar)]
public class SerAuxErrBar : OfficeGraphBiffRecord
{
    public enum ErrorAmoutType
    {
        Percentage = 1,
        FixedValue = 2,
        StandardDeviation = 3,
        StandardError = 5
    }
    
    public enum ErrorBarDirection
    {
        HorizontalPositive = 1,
        HorizontalNegative = 2,
        VerticalPositive = 3,
        VerticalNegative = 4
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.SerAuxErrBar;
    /// <summary>
    ///     Specifies the error amount type of the error bars.
    /// </summary>
    public ErrorAmoutType ebsrc;
    /// <summary>
    ///     A Boolean that specifies whether the error bars are T-shaped.
    /// </summary>
    public bool fTeeTop;
    /// <summary>
    ///     An Xnum that specifies the fixed value, percentage, or number of standard deviations for the error bars.
    ///     If ebsrc is StandardError this MUST be ignored.
    /// </summary>
    public double numValue;
    /// <summary>
    ///     Specifies the direction of the error bars.
    /// </summary>
    public ErrorBarDirection sertm;
    
    public SerAuxErrBar(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        sertm = (ErrorBarDirection)reader.ReadByte();
        ebsrc = (ErrorAmoutType)reader.ReadByte();
        fTeeTop = Utils.ByteToBool(reader.ReadByte());
        reader.ReadByte(); // reserved
        numValue = reader.ReadDouble();
        reader.ReadBytes(2); //unused
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}