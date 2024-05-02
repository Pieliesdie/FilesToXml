using System;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class PictureBulletInformation
{
    public bool fDefaultPic;
    public bool fFormatting;
    public bool fNoAutoSize;
    public bool fPicBullet;
    public bool fTemporary;
    public int iBullet;
    
    public PictureBulletInformation(byte[] bytes)
    {
        if (bytes.Length == 6)
        {
            var flag = BitConverter.ToInt16(bytes, 0);
            fPicBullet = Utils.BitmaskToBool(flag, 0x0001);
            fNoAutoSize = Utils.BitmaskToBool(flag, 0x0002);
            fDefaultPic = Utils.BitmaskToBool(flag, 0x0004);
            fTemporary = Utils.BitmaskToBool(flag, 0x0008);
            fFormatting = Utils.BitmaskToBool(flag, 0x1000);
            iBullet = BitConverter.ToInt32(bytes, 2);
        }
        else
        {
            throw new ByteParseException("Cannot parse the struct PBI, the length of the struct doesn't match");
        }
    }
}