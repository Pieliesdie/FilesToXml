using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class BreakDescriptor : ByteStructure
{
    /// <summary>
    ///     Number of cp's considered for this break; note that the CP's described by cpDepend in this break reside in the next
    ///     BKD
    /// </summary>
    public short dcpDepend;
    /// <summary>
    ///     When true, this indicates that this is a column break.
    /// </summary>
    public bool fColumnBreak;
    /// <summary>
    ///     Used temporarily while Word is running.
    /// </summary>
    public bool fMarked;
    /// <summary>
    ///     When true, this indicates that this is a table break.
    /// </summary>
    public bool fTableBreak;
    /// <summary>
    ///     In textbox BKD, when true indicates that text overflows the end of this textbox
    /// </summary>
    public bool fTextOverflow;
    /// <summary>
    ///     In textbox BKD, when true indicates cpLim of this textbox is not valid
    /// </summary>
    public bool fUnk;
    /// <summary>
    /// </summary>
    public ushort icol;
    /// <summary>
    ///     Except in textbox BKD, index to PGD in plfpgd that describes the page this break is on
    /// </summary>
    public short ipgd;
    /// <summary>
    ///     In textbox BKD
    /// </summary>
    public short itxbxs;
    
    public BreakDescriptor(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        ipgd = reader.ReadInt16();
        itxbxs = ipgd;
        dcpDepend = reader.ReadInt16();
        int flag = reader.ReadInt16();
        icol = (ushort)Utils.BitmaskToInt(flag, 0x00FF);
        fTableBreak = Utils.BitmaskToBool(flag, 0x0100);
        fColumnBreak = Utils.BitmaskToBool(flag, 0x0200);
        fMarked = Utils.BitmaskToBool(flag, 0x0400);
        fUnk = Utils.BitmaskToBool(flag, 0x0800);
        fTextOverflow = Utils.BitmaskToBool(flag, 0x1000);
    }
}