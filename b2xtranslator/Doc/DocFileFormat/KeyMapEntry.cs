using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class KeyMapEntry : ByteStructure
{
    public enum ActionType
    {
        ktCid,
        ktChar,
        ktMask
    }
    
    private const int KME_LENGTH = 14;
    /// <summary>
    /// </summary>
    public short kcm1;
    /// <summary>
    /// </summary>
    public short kcm2;
    /// <summary>
    /// </summary>
    public ActionType kt;
    /// <summary>
    /// </summary>
    public char paramChar;
    public CommandIdentifier paramCid;
    
    public KeyMapEntry(VirtualStreamReader reader)
        : base(reader, KME_LENGTH)
    {
        //ignore the first 4 bytes
        reader.ReadBytes(4);
        
        //Primary KCM
        kcm1 = reader.ReadInt16();
        
        //Secondary KCM
        kcm2 = reader.ReadInt16();
        
        //Key Action Type
        kt = (ActionType)reader.ReadInt16();
        
        //read the params
        switch (kt)
        {
            case ActionType.ktCid:
                paramCid = new CommandIdentifier(reader);
                break;
            case ActionType.ktChar:
                paramChar = (char)reader.ReadInt32();
                break;
            default:
                reader.ReadBytes(4);
                break;
        }
    }
}