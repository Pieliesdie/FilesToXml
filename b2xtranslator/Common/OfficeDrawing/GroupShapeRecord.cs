using System;
using System.Drawing;
using System.IO;

namespace b2xtranslator.OfficeDrawing;

[OfficeRecord(0xF009)]
public class GroupShapeRecord : Record
{
    /// <summary>
    /// </summary>
    public Rectangle rcgBounds;
    
    public GroupShapeRecord(BinaryReader _reader, uint size, uint typeCode, uint version, uint instance)
        : base(_reader, size, typeCode, version, instance)
    {
        //read the rectangle (16 bytes)
        var left = Math.Max(0, Reader.ReadInt32());
        var top = Math.Max(0, Reader.ReadInt32());
        var right = Math.Max(0, Reader.ReadInt32());
        var bottom = Math.Max(0, Reader.ReadInt32());
        
        rcgBounds = new Rectangle(
            new Point(left, top),
            new Size(right - left, bottom - top)
        );
    }
}