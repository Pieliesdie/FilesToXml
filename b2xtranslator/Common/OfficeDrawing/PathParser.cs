using System;
using System.Collections.Generic;
using System.Drawing;
using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class GD
{
    public bool fCalculatedParam1;
    public bool fCalculatedParam2;
    public bool fCalculatedParam3;
    public short param1;
    public short param2;
    public short param3;
    public int sgf;
    
    public GD(ushort flags, short p1, short p2, short p3)
    {
        sgf = flags & 0x1FFF;
        
        fCalculatedParam1 = Utils.BitmaskToBool(flags, 0x1 << 13);
        fCalculatedParam2 = Utils.BitmaskToBool(flags, 0x1 << 14);
        fCalculatedParam3 = Utils.BitmaskToBool(flags, 0x1 << 15);
        
        param1 = p1;
        param2 = p2;
        param3 = p3;
    }
}

public class PathParser
{
    public ushort cbElemVert;
    public PathParser(byte[] pSegmentInfo, byte[] pVertices) : this(pSegmentInfo, pVertices, null) { }
    
    public PathParser(byte[] pSegmentInfo, byte[] pVertices, byte[] pGuides)
    {
        Guides = new List<GD>();
        
        if (pGuides != null && pGuides.Length > 0)
        {
            var nElemsG = BitConverter.ToUInt16(pGuides, 0);
            var nElemsAllocG = BitConverter.ToUInt16(pGuides, 2);
            var cbElemG = BitConverter.ToUInt16(pGuides, 4);
            for (var i = 6; i < pGuides.Length; i += cbElemG)
            {
                Guides.Add(new GD(BitConverter.ToUInt16(pGuides, i), BitConverter.ToInt16(pGuides, i + 2), BitConverter.ToInt16(pGuides, i + 4), BitConverter.ToInt16(pGuides, i + 6)));
            }
        }
        
        // parse the segments
        Segments = new List<PathSegment>();
        if (pSegmentInfo != null && pSegmentInfo.Length > 0)
        {
            for (var i = 6; i < pSegmentInfo.Length; i += 2)
            {
                Segments.Add(
                    new PathSegment(
                        BitConverter.ToUInt16(pSegmentInfo, i)
                    ));
            }
        }
        
        // parse the values
        Values = new List<Point>();
        cbElemVert = BitConverter.ToUInt16(pVertices, 4);
        if (cbElemVert == 0xfff0)
        {
            cbElemVert = 4;
        }
        
        int x;
        int y;
        for (var i = 6; i <= pVertices.Length - cbElemVert; i += cbElemVert)
        {
            switch (cbElemVert)
            {
                case 4:
                    x = BitConverter.ToInt16(pVertices, i);
                    
                    if (x < 0) { }
                    
                    y = BitConverter.ToInt16(pVertices, i + cbElemVert / 2);
                    Values.Add(new Point(x, y));
                    break;
                case 8:
                    x = BitConverter.ToInt32(pVertices, i);
                    
                    if (x < 0)
                    {
                        if ((uint)x > 0x80000000 && (uint)x <= 0x8000007F)
                        {
                            var index = (uint)x - 0x80000000;
                            //TODO
                        }
                    }
                    
                    y = BitConverter.ToInt32(pVertices, i + cbElemVert / 2);
                    Values.Add(
                        new Point(x, y));
                    break;
            }
        }
    }
    
    public List<Point> Values { get; set; }
    public List<GD> Guides { get; set; }
    public List<PathSegment> Segments { get; set; }
}