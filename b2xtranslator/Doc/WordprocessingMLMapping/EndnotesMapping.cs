using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class EndnotesMapping : DocumentMapping
{
    public EndnotesMapping(ConversionContext ctx)
        : base(ctx, ctx.Docx.MainDocumentPart.EndnotesPart)
    {
        _ctx = ctx;
    }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        var id = 0;
        
        _writer.WriteStartElement("w", "endnotes", OpenXmlNamespaces.WordprocessingML);
        
        var cp = doc.FIB.ccpText + doc.FIB.ccpFtn + doc.FIB.ccpHdr + doc.FIB.ccpAtn;
        var cpEnd = doc.FIB.ccpText + doc.FIB.ccpFtn + doc.FIB.ccpHdr + doc.FIB.ccpAtn + doc.FIB.ccpEdn - 2;
        while (cp < cpEnd)
        {
            _writer.WriteStartElement("w", "endnote", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "id", OpenXmlNamespaces.WordprocessingML, id.ToString());
            cp = writeParagraph(cp);
            _writer.WriteEndElement();
            id++;
        }
        
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
}