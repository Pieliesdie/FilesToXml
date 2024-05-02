using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class FootnotesMapping : DocumentMapping
{
    public FootnotesMapping(ConversionContext ctx)
        : base(ctx, ctx.Docx.MainDocumentPart.FootnotesPart)
    {
        _ctx = ctx;
    }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        var id = 0;
        
        _writer.WriteStartElement("w", "footnotes", OpenXmlNamespaces.WordprocessingML);
        
        var cp = doc.FIB.ccpText;
        while (cp < doc.FIB.ccpText + doc.FIB.ccpFtn - 2)
        {
            _writer.WriteStartElement("w", "footnote", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "id", OpenXmlNamespaces.WordprocessingML, id.ToString());
            cp = writeParagraph(cp);
            _writer.WriteEndElement();
            id++;
        }
        
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
}