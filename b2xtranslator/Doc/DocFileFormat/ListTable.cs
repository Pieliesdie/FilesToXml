using System.Collections.Generic;
using System.IO;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class ListTable : List<ListData>, IVisitable
{
    public ListTable(FileInformationBlock fib, VirtualStream tableStream)
    {
        if (fib.lcbPlfLst > 0)
        {
            var reader = new VirtualStreamReader(tableStream);
            reader.BaseStream.Seek(fib.fcPlfLst, SeekOrigin.Begin);
            
            //the ListTable is not a real plex:
            //it starts with a count, followed by the array of LSTF structs,
            //followed by the array of LVLF structs
            
            //read count
            var count = reader.ReadInt16();
            
            //read the LSTF structs
            for (var i = 0; i < count; i++)
            {
                Add(new ListData(reader, ByteStructure.VARIABLE_LENGTH));
            }
            
            //read the LVLF structs
            for (var i = 0; i < count; i++)
            {
                var lstf = this[i];
                for (var j = 0; j < lstf.rglvl.Length; j++)
                {
                    lstf.rglvl[j] = new ListLevel(reader, ByteStructure.VARIABLE_LENGTH);
                }
            }
        }
    }
    
    #region IVisitable Members
    
    public void Convert<T>(T mapping)
    {
        ((IMapping<ListTable>)mapping).Apply(this);
    }
    
    #endregion
}