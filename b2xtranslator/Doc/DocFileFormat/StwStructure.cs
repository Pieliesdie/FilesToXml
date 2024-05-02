using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public sealed class StwStructure : Dictionary<string, string>
{
    public StwStructure(VirtualStream tableStream, uint fc, uint lcb)
    {
        tableStream.Seek(fc, SeekOrigin.Begin);
        
        // parse the names
        var names = new StringTable(typeof(string), new VirtualStreamReader(tableStream));
        
        // parse the values
        var values = new List<string>();
        while (tableStream.Position < fc + lcb)
        {
            values.Add(Utils.ReadXst(tableStream));
        }
        
        // map to the dictionary
        if (names.Strings.Count == values.Count)
        {
            for (var i = 0; i < names.Strings.Count; i++)
            {
                Add(names.Strings[i], values[i]);
            }
        }
    }
}