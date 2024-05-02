using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class AnnotationReferenceExtraTable : List<AnnotationReferenceDescriptorExtra>
{
    private const int ARTDPost10_LENGTH = 16;
    
    public AnnotationReferenceExtraTable(FileInformationBlock fib, VirtualStream tableStream)
    {
        if (fib.nFib >= FileInformationBlock.FibVersion.Fib2002)
        {
            tableStream.Seek(fib.fcAtrdExtra, SeekOrigin.Begin);
            var reader = new VirtualStreamReader(tableStream);
            
            var n = (int)fib.lcbAtrdExtra / ARTDPost10_LENGTH;
            
            //read the n ATRDPost10 structs
            for (var i = 0; i < n; i++)
            {
                Add(new AnnotationReferenceDescriptorExtra(reader, ARTDPost10_LENGTH));
            }
        }
    }
}