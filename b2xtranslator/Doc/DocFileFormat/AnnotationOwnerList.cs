using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;
using b2xtranslator.Tools;

namespace b2xtranslator.doc.DocFileFormat;

public class AnnotationOwnerList : List<string>
{
    public AnnotationOwnerList(FileInformationBlock fib, VirtualStream tableStream)
    {
        tableStream.Seek(fib.fcGrpXstAtnOwners, SeekOrigin.Begin);
        
        while (tableStream.Position < fib.fcGrpXstAtnOwners + fib.lcbGrpXstAtnOwners)
        {
            Add(Utils.ReadXst(tableStream));
        }
    }
}