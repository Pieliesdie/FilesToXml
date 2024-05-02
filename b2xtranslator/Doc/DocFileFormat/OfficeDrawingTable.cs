using System;
using System.Collections.Generic;
using System.IO;
using b2xtranslator.StructuredStorage.Reader;

namespace b2xtranslator.doc.DocFileFormat;

public class OfficeDrawingTable : Dictionary<int, FileShapeAddress>
{
    public enum OfficeDrawingTableType
    {
        Header,
        MainDocument
    }
    
    private const int FSPA_LENGTH = 26;
    
    public OfficeDrawingTable(WordDocument doc, OfficeDrawingTableType type)
    {
        var reader = new VirtualStreamReader(doc.TableStream);
        
        //FSPA has size 26 + 4 byte for the FC = 30 byte
        var n = 0;
        uint startFc = 0;
        if (type == OfficeDrawingTableType.MainDocument)
        {
            startFc = doc.FIB.fcPlcSpaMom;
            n = (int)Math.Floor((double)doc.FIB.lcbPlcSpaMom / 30);
        }
        else if (type == OfficeDrawingTableType.Header)
        {
            startFc = doc.FIB.fcPlcSpaHdr;
            n = (int)Math.Floor((double)doc.FIB.lcbPlcSpaHdr / 30);
        }
        
        //there are n+1 FCs ...
        doc.TableStream.Seek(startFc, SeekOrigin.Begin);
        var fcs = new int[n + 1];
        for (var i = 0; i < n + 1; i++)
        {
            fcs[i] = reader.ReadInt32();
        }
        
        //followed by n FSPAs
        for (var i = 0; i < n; i++)
        {
            FileShapeAddress fspa = null;
            if (type == OfficeDrawingTableType.Header)
            {
                //fspa = new FileShapeAddress(reader, doc.OfficeArtContent);
            }
            else if (type == OfficeDrawingTableType.MainDocument)
            {
                //fspa = new FileShapeAddress(reader, doc.OfficeArtContent);
            }
            
            Add(fcs[i], fspa);
        }
    }
}