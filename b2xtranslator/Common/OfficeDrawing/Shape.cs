using System.IO;
using b2xtranslator.OfficeDrawing.Shapetypes;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF00A)]
public class Shape : Record
{
    /// <summary>
    ///     Background shape
    /// </summary>
    public bool fBackground;
    /// <summary>
    ///     Not a top-level shape
    /// </summary>
    public bool fChild;
    /// <summary>
    ///     Connector type of shape
    /// </summary>
    public bool fConnector;
    /// <summary>
    ///     The shape has been deleted
    /// </summary>
    public bool fDeleted;
    /// <summary>
    ///     Shape is flipped horizontally
    /// </summary>
    public bool fFlipH;
    /// <summary>
    ///     Shape is flipped vertically
    /// </summary>
    public bool fFlipV;
    /// <summary>
    ///     This shape is a group shape
    /// </summary>
    public bool fGroup;
    /// <summary>
    ///     Shape has an anchor of some kind
    /// </summary>
    public bool fHaveAnchor;
    /// <summary>
    ///     Shape has a hspMaster property
    /// </summary>
    public bool fHaveMaster;
    /// <summary>
    ///     Shape has a shape type property
    /// </summary>
    public bool fHaveSpt;
    /// <summary>
    ///     The shape is an OLE object
    /// </summary>
    public bool fOleShape;
    /// <summary>
    ///     This is the topmost group shape.<br />
    ///     Exactly one of these per drawing.
    /// </summary>
    public bool fPatriarch;
    /// <summary>
    ///     The shape type of the shape
    /// </summary>
    public ShapeType ShapeType;
    public int spid;
    
    public Shape(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        spid = Reader.ReadInt32();
        
        var flag = Reader.ReadUInt32();
        fGroup = Utils.BitmaskToBool(flag, 0x1);
        fChild = Utils.BitmaskToBool(flag, 0x2);
        fPatriarch = Utils.BitmaskToBool(flag, 0x4);
        fDeleted = Utils.BitmaskToBool(flag, 0x8);
        fOleShape = Utils.BitmaskToBool(flag, 0x10);
        fHaveMaster = Utils.BitmaskToBool(flag, 0x20);
        fFlipH = Utils.BitmaskToBool(flag, 0x40);
        fFlipV = Utils.BitmaskToBool(flag, 0x80);
        fConnector = Utils.BitmaskToBool(flag, 0x100);
        fHaveAnchor = Utils.BitmaskToBool(flag, 0x200);
        fBackground = Utils.BitmaskToBool(flag, 0x400);
        fHaveSpt = Utils.BitmaskToBool(flag, 0x800);
        ShapeType = ShapeType.GetShapeType(Instance);
    }
    
    public override string ToString(uint depth)
    {
        return string.Format("{0}\n{1}Id = {2}, isGroup = {3}, isChild = {4}, isPatriarch = {5}",
            base.ToString(depth), IndentationForDepth(depth + 1),
            spid,
            fGroup, fChild, fPatriarch);
    }
}