using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class ToolbarDelta : ByteStructure
{
    public const int TBDelta_LENGTH = 18;
    /// <summary>
    ///     Unsigned integer that specifies the size, in bytes, of the toolbar control associated to this TBDelta. <br />
    ///     This field MUST only be used when fOnDisk equals 1.<br />
    ///     If fOnDisk equals 0, value MUST be 0x0000.
    /// </summary>
    public ushort cbTBC;
    /// <summary>
    ///     Structure of type Cid that specifies the Word‘s command identifier for the
    ///     toolbar control associated to this TBDelta. <br />
    ///     Toolbar controls MUST only have Cid structures that have Cmt values equal to 0x0001 or 0x0003.
    /// </summary>
    public int cid;
    /// <summary>
    ///     Signed integer. Refer to the following table for the value that this field MUST have:
    /// </summary>
    public int cidNext;
    public int dopr;
    public bool fAtEnd;
    /// <summary>
    ///     Unsigned integer that specifies the file offset in the Table Stream where the
    ///     toolbar control associated to this TBDelta is stored. <br />
    ///     Value MUST be 0x00000000 if fOnDisk is not equal to 1.
    /// </summary>
    public int fc;
    /// <summary>
    ///     A bit that specifies if the toolbar control associated to this TBDelta does not drop a menu toolbar.<br />
    ///     A value of 1 specifies that the toolbar control associated to this TBDelta does not drop a custom menu toolbar.
    ///     <br />
    ///     Value MUST be 0 if the toolbar control associated to this TBDelta is not a custom toolbar
    ///     control that drops a custom menu toolbar or if dopr does not equal 1.
    /// </summary>
    public bool fDead;
    /// <summary>
    ///     A bit that specifies if a toolbar control associated to this TBDelta has been written to the file. <br />
    ///     A value of 1 specifies that a toolbar control associated to this TBDelta has been written to the file. <br />
    ///     MUST be 1 if dopr equals 0 or 1.
    /// </summary>
    public bool fOnDisk;
    /// <summary>
    ///     Unsigned integer that specifies the zero-based index of the toolbar control
    ///     associated to this TBDelta in the toolbar at the time the toolbar delta was created. <br />
    ///     It is possible for more than one TBDelta structure, that affects the same toolbar,
    ///     to have the same value in the ibts field because this field specifies the index of
    ///     the toolbar control associated to the TBDelta in the toolbar at the time the toolbar delta was created.
    /// </summary>
    public byte ibts;
    /// <summary>
    ///     This field MUST only be used when the toolbar control associated to this TBDelta is a
    ///     custom toolbar control that drops a custom menu toolbar. <br />
    ///     Unsigned integer that specifies the index to the Customization structure,
    ///     contained in the rCustomizations array, that also contains the Customization
    ///     that contains the customizationData array that contains this structure,
    ///     that contains the CTB structure that specifies the custom menu toolbar dropped by
    ///     the toolbar control associated to this TBDelta. MUST be 0 if the toolbar control
    ///     associated to this TBDelta is not a custom toolbar control that drops a custom menu toolbar. <br />
    ///     Value MUST be greater or equal to 0 and SHOULD <256> be less than the value of the cCust field of
    ///     the CTBWRAPPER structure that contains the rCustomizations array that contains the Customization
    ///     structure that contains the customizationData array that contains this structure.
    /// </summary>
    public int iTB;
    
    public ToolbarDelta(VirtualStreamReader reader)
        : base(reader, TBDelta_LENGTH)
    {
        var flags1 = reader.ReadByte();
        dopr = Utils.BitmaskToInt(flags1, 0x03);
        fAtEnd = Utils.BitmaskToBool(flags1, 0x04);
        
        ibts = reader.ReadByte();
        cidNext = reader.ReadInt32();
        cid = reader.ReadInt32();
        fc = reader.ReadInt32();
        
        var flags2 = reader.ReadUInt16();
        fOnDisk = Utils.BitmaskToBool(flags2, 0x0001);
        iTB = Utils.BitmaskToInt(flags2, 0x3FFE);
        fDead = Utils.BitmaskToBool(flags2, 0x8000);
        
        cbTBC = reader.ReadUInt16();
    }
}