using System.Collections.Generic;
using System.IO;
using System.Text;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class CommandTable : IVisitable
{
    private readonly bool breakWhile;
    public StringTable CommandStringTable;
    public CustomToolbarWrapper CustomToolbars;
    public List<KeyMapEntry> KeyMapEntries;
    public List<MacroData> MacroDatas;
    public Dictionary<int, string> MacroNames;
    
    public CommandTable(FileInformationBlock fib, VirtualStream tableStream)
    {
        tableStream.Seek(fib.fcCmds, SeekOrigin.Begin);
        var reader = new VirtualStreamReader(tableStream);
        
        //byte[] bytes = reader.ReadBytes((int)fib.lcbCmds);
        MacroDatas = new List<MacroData>();
        KeyMapEntries = new List<KeyMapEntry>();
        MacroNames = new Dictionary<int, string>();
        
        //skip the version
        reader.ReadByte();
        
        //parse the commandtable
        while (reader.BaseStream.Position < fib.fcCmds + fib.lcbCmds && !breakWhile)
        {
            //read the type
            var ch = reader.ReadByte();
            
            switch (ch)
            {
                case 0x1:
                    //it's a PlfMcd
                    var iMacMcd = reader.ReadInt32();
                    for (var i = 0; i < iMacMcd; i++)
                    {
                        MacroDatas.Add(new MacroData(reader));
                    }
                    
                    break;
                case 0x2:
                    //it's a PlfAcd
                    //skip the ACDs
                    var iMacAcd = reader.ReadInt32();
                    reader.ReadBytes(iMacAcd * 4);
                    break;
                case 0x3:
                    //Keymap Entries
                    var iMacKme = reader.ReadInt32();
                    for (var i = 0; i < iMacKme; i++)
                    {
                        KeyMapEntries.Add(new KeyMapEntry(reader));
                    }
                    
                    break;
                case 0x4:
                    //Keymap Entries
                    var iMacKmeInvalid = reader.ReadInt32();
                    for (var i = 0; i < iMacKmeInvalid; i++)
                    {
                        KeyMapEntries.Add(new KeyMapEntry(reader));
                    }
                    
                    break;
                case 0x10:
                    //it's a TcgSttbf
                    CommandStringTable = new StringTable(typeof(string), reader);
                    break;
                case 0x11:
                    //it's a MacroNames table
                    int iMacMn = reader.ReadInt16();
                    for (var i = 0; i < iMacMn; i++)
                    {
                        var ibst = reader.ReadInt16();
                        var cch = reader.ReadInt16();
                        MacroNames[ibst] = Encoding.Unicode.GetString(reader.ReadBytes(cch * 2));
                        //skip the terminating zero
                        reader.ReadBytes(2);
                    }
                    
                    break;
                case 0x12:
                    //it's a CTBWRAPPER structure
                    CustomToolbars = new CustomToolbarWrapper(reader);
                    break;
                default:
                    breakWhile = true;
                    break;
            }
        }
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<CommandTable>)mapping).Apply(this);
    }
    
    #endregion
}