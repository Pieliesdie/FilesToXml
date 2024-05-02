using b2xtranslator.Tools;

namespace b2xtranslator.OfficeDrawing;

public class BlipBooleanProperties
{
    public bool fLooping;
    public bool fNoHitTestPicture;
    public bool fPictureActive;
    public bool fPictureBiLevel;
    public bool fPictureGray;
    public bool fPicturePreserveGrays;
    public bool fRewind;
    public bool fusefLooping;
    public bool fusefNoHitTestPicture;
    public bool fusefPictureActive;
    public bool fusefPictureBiLevel;
    public bool fusefPictureGray;
    public bool fusefPicturePreserveGrays;
    public bool fusefRewind;
    
    public BlipBooleanProperties(uint entryOperand)
    {
        fPictureActive = Utils.BitmaskToBool(entryOperand, 0x1);
        fPictureBiLevel = Utils.BitmaskToBool(entryOperand, 0x1 << 1);
        fPictureGray = Utils.BitmaskToBool(entryOperand, 0x1 << 2);
        fNoHitTestPicture = Utils.BitmaskToBool(entryOperand, 0x1 << 3);
        fLooping = Utils.BitmaskToBool(entryOperand, 0x1 << 4);
        fRewind = Utils.BitmaskToBool(entryOperand, 0x1 << 5);
        fPicturePreserveGrays = Utils.BitmaskToBool(entryOperand, 0x1 << 6);
        //unused 9 bits
        fusefPictureActive = Utils.BitmaskToBool(entryOperand, 0x1 << 16);
        fusefPictureBiLevel = Utils.BitmaskToBool(entryOperand, 0x1 << 17);
        fusefPictureGray = Utils.BitmaskToBool(entryOperand, 0x1 << 18);
        fusefNoHitTestPicture = Utils.BitmaskToBool(entryOperand, 0x1 << 19);
        fusefLooping = Utils.BitmaskToBool(entryOperand, 0x1 << 20);
        fusefRewind = Utils.BitmaskToBool(entryOperand, 0x1 << 21);
        fusefPicturePreserveGrays = Utils.BitmaskToBool(entryOperand, 0x1 << 22);
        //unused 9 bits
    }
}