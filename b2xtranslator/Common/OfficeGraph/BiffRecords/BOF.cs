using System.Diagnostics;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeGraph.BiffRecords;

/// <summary>
///     This record specifies properties about the substream and specifies the beginning
///     of a collection of records as defined by the Workbook Stream ABNF and the Chart Sheet Substream ABNF.
/// </summary>
[OfficeGraphBiffRecord(GraphRecordNumber.BOF)]
public class BOF : OfficeGraphBiffRecord
{
    public enum DocType : ushort
    {
        Workbook = 0x005,
        ChartSheet = 0x8000
    }
    
    public const GraphRecordNumber ID = GraphRecordNumber.BOF;
    /// <summary>
    ///     An unsigned integer that specifies the type of data contained in the substream.
    ///     MUST be a value from the following table:
    ///     Value     Meaning
    ///     0x0005    Specifies a workbook stream.
    ///     0x8000    Specifies a chart sheet substream.
    /// </summary>
    public DocType docType;
    /// <summary>
    ///     A bit that specifies whether this substream was last edited by a beta version of the application.
    ///     MUST be 0.
    /// </summary>
    public bool fBeta;
    /// <summary>
    ///     A bit that specifies whether this substream has ever been edited by a beta version of the application.
    ///     MUST be 0.
    /// </summary>
    public bool fBetaAny;
    /// <summary>
    ///     A bit that specifies that whether this substream has hit the 255 font limit, such that new Font records cannot be
    ///     added to it.
    /// </summary>
    public bool fFontLimit;
    /// <summary>
    ///     A bit that specifies whether this substream caused an out-of-memory failure while loading charting or graphics
    ///     data.
    /// </summary>
    public bool fGlJmp;
    /// <summary>
    ///     A bit that specifies whether this substream has ever been edited on a Macintosh platform.
    ///     MUST be 0.
    /// </summary>
    public bool fMacAny;
    /// <summary>
    ///     A bit that specifies whether this substream caused an out-of-memory failure.
    /// </summary>
    public bool fOOM;
    /// <summary>
    ///     A bit that specifies whether the substream was last edited on a RISC platform.
    ///     MUST be 0.
    /// </summary>
    public bool fRisc;
    /// <summary>
    ///     A bit that specifies whether this substream has ever been edited on a RISC platform.
    ///     MUST be 0.
    /// </summary>
    public bool fRiscAny;
    /// <summary>
    ///     A bit that specifies whether this substream was last edited on a Windows platform.
    ///     MUST be 1.
    /// </summary>
    public bool fWin;
    /// <summary>
    ///     A bit that specifies whether this substream has ever been edited on a Windows platform.
    ///     MUST be 1.
    /// </summary>
    public bool fWinAny;
    /// <summary>
    ///     An unsigned integer that specifies the build identifier of the application creating the substream.
    /// </summary>
    public ushort rupBuild;
    /// <summary>
    ///     An unsigned integer that specifies the version of the file format.
    ///     This value MUST be 0x07CC or 0x07CD.
    ///     This value SHOULD be 0x07CD (1997).
    /// </summary>
    public ushort rupYear;
    /// <summary>
    ///     An unsigned integer (4 bits) that specifies the application version that saved this substream most recently. The
    ///     value MUST be the value of verXLHigh field or less.
    /// </summary>
    public byte verLastXLSaved;
    /// <summary>
    ///     An unsigned integer that specifies the version of the file format.
    ///     MUST be 0x06.
    /// </summary>
    public byte verLowestBiff;
    /// <summary>
    ///     An unsigned integer that specifies the version of the substream.
    ///     MUST be 0x0680.
    /// </summary>
    public ushort version;
    /// <summary>
    ///     An unsigned integer (4 bits) that specifies the highest version of the application that has ever saved this
    ///     substream.
    /// </summary>
    public byte verXLHigh;
    
    public BOF(IStreamReader reader, GraphRecordNumber id, ushort length)
        : base(reader, id, length)
    {
        // assert that the correct record type is instantiated
        Debug.Assert(Id == ID);
        
        // initialize class members from stream
        version = reader.ReadUInt16();
        docType = (DocType)reader.ReadUInt16();
        rupBuild = reader.ReadUInt16();
        rupYear = reader.ReadUInt16();
        
        var flags = reader.ReadUInt32();
        fWin = Utils.BitmaskToBool(flags, 0x0001);
        fRisc = Utils.BitmaskToBool(flags, 0x0002);
        fBeta = Utils.BitmaskToBool(flags, 0x0004);
        fWinAny = Utils.BitmaskToBool(flags, 0x0008);
        fMacAny = Utils.BitmaskToBool(flags, 0x0010);
        fBetaAny = Utils.BitmaskToBool(flags, 0x0020);
        // 2 bits ignored
        fRiscAny = Utils.BitmaskToBool(flags, 0x0100);
        fOOM = Utils.BitmaskToBool(flags, 0x0200);
        fGlJmp = Utils.BitmaskToBool(flags, 0x0400);
        // 2 bits ignored
        fFontLimit = Utils.BitmaskToBool(flags, 0x2000);
        verXLHigh = Utils.BitmaskToByte(flags, 0x0003C000);
        
        verLowestBiff = reader.ReadByte();
        verLastXLSaved = Utils.BitmaskToByte(reader.ReadUInt16(), 0x00FF);
        
        // ignore remaing part of record
        reader.ReadByte();
        
        // assert that the correct number of bytes has been read from the stream
        Debug.Assert(Offset + Length == Reader.BaseStream.Position);
    }
}