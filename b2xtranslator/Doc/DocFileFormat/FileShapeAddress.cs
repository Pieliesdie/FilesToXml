using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class FileShapeAddress : ByteStructure
{
    public enum AnchorType
    {
        margin,
        page,
        text
    }
    
    /// <summary>
    ///     X position of shape relative to anchor CP<br />
    ///     0 relative to page margin<br />
    ///     1 relative to top of page<br />
    ///     2 relative to text (column for horizontal text; paragraph for vertical text)<br />
    ///     3 reserved for future use
    /// </summary>
    public AnchorType bx;
    /// <summary>
    ///     Y position of shape relative to anchor CP<br />
    ///     0 relative to page margin<br />
    ///     1 relative to top of page<br />
    ///     2 relative to text (column for horizontal text; paragraph for vertical text)<br />
    ///     3 reserved for future use
    /// </summary>
    public AnchorType by;
    /// <summary>
    ///     Count of textboxes in shape (undo doc only)
    /// </summary>
    public int cTxbx;
    /// <summary>
    ///     true: anchor is locked <br />
    ///     fasle: anchor is not locked
    /// </summary>
    public bool fAnchorLock;
    /// <summary>
    ///     true: shape is below text <br />
    ///     false: shape is above text
    /// </summary>
    public bool fBelowText;
    /// <summary>
    ///     true in the undo doc when shape is from the header doc<br />
    ///     false otherwise (undefined when not in the undo doc)
    /// </summary>
    public bool fHdr;
    /// <summary>
    ///     When set, temporarily overrides bx, by,
    ///     forcing the xaLeft, xaRight, yaTop, and yaBottom fields
    ///     to all be page relative.
    /// </summary>
    public bool fRcaSimple;
    /// <summary>
    ///     Shape Identifier. Used in conjunction with the office art data
    ///     (found via fcDggInfo in the FIB) to find the actual data for this shape.
    /// </summary>
    public int spid;
    /// <summary>
    ///     Text wrapping mode <br />
    ///     0 like 2, but doesn‘t require absolute object <br />
    ///     1 no text next to shape <br />
    ///     2 wrap around absolute object <br />
    ///     3 wrap as if no object present <br />
    ///     4 wrap tightly around object <br />
    ///     5 wrap tightly, but allow holes <br />
    ///     6-15 reserved for future use
    /// </summary>
    public ushort wr;
    /// <summary>
    ///     Text wrapping mode type (valid only for wrapping modes 2 and 4)<br />
    ///     0 wrap both sides <br />
    ///     1 wrap only on left <br />
    ///     2 wrap only on right <br />
    ///     3 wrap only on largest side
    /// </summary>
    public ushort wrk;
    /// <summary>
    ///     Left of rectangle enclosing shape relative to the origin of the shape
    /// </summary>
    public int xaLeft;
    /// <summary>
    ///     Right of rectangle enclosing shape relative to the origin of the shape
    /// </summary>
    public int xaRight;
    /// <summary>
    ///     Bottom of the rectangle enclosing shape relative to the origin of the shape
    /// </summary>
    public int yaBottom;
    /// <summary>
    ///     Top of rectangle enclosing shape relative to the origin of the shape
    /// </summary>
    public int yaTop;
    
    /// <summary>
    /// </summary>
    /// <param name="reader"></param>
    public FileShapeAddress(VirtualStreamReader reader, int length)
        : base(reader, length)
    {
        spid = _reader.ReadInt32();
        xaLeft = _reader.ReadInt32();
        yaTop = _reader.ReadInt32();
        xaRight = _reader.ReadInt32();
        yaBottom = _reader.ReadInt32();
        
        var flag = _reader.ReadUInt16();
        fHdr = Utils.BitmaskToBool(flag, 0x0001);
        bx = (AnchorType)Utils.BitmaskToInt(flag, 0x0006);
        by = (AnchorType)Utils.BitmaskToInt(flag, 0x0018);
        wr = (ushort)Utils.BitmaskToInt(flag, 0x01E0);
        wrk = (ushort)Utils.BitmaskToInt(flag, 0x1E00);
        fRcaSimple = Utils.BitmaskToBool(flag, 0x2000);
        fBelowText = Utils.BitmaskToBool(flag, 0x4000);
        fAnchorLock = Utils.BitmaskToBool(flag, 0x8000);
        
        cTxbx = _reader.ReadInt32();
    }
}