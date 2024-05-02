using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class MacroData : ByteStructure
{
    private const int MCD_LENGTH = 24;
    /// <summary>
    ///     Unsigned integer that specifies the name of the macro.<br />
    ///     Macro name is specified by MacroName.xstz of the MacroName entry in
    ///     the MacroNames such that MacroName.ibst equals ibst. <br />
    ///     MacroNames MUST contain such an entry.
    /// </summary>
    public short ibst;
    /// <summary>
    ///     An unsigned integer that specifies the index into the
    ///     CommandStringTable (CommandTable.CommandStringTable)
    ///     where the macro‘s name and arguments are specified.
    /// </summary>
    public short ibstName;
    
    public MacroData(VirtualStreamReader reader)
        : base(reader, MCD_LENGTH)
    {
        //first 2 bytes are reserved
        reader.ReadBytes(2);
        
        ibst = reader.ReadInt16();
        
        ibstName = reader.ReadInt16();
        
        //last 18 bytes are reserved
        reader.ReadBytes(18);
    }
}