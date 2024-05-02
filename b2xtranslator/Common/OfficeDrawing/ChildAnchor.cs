using System.Drawing;
using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF00F)]
public class ChildAnchor : Record
{
    public int Bottom;
    public int Left;
    /// <summary>
    ///     Rectangle that describe sthe bounds of the anchor
    /// </summary>
    public Rectangle rcgBounds;
    public int Right;
    public int Top;
    
    public ChildAnchor(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        Left = Reader.ReadInt32();
        Top = Reader.ReadInt32();
        Right = Reader.ReadInt32();
        Bottom = Reader.ReadInt32();
        rcgBounds = new Rectangle(
            new Point(Left, Top),
            new Size(Right - Left, Bottom - Top)
        );
    }
}