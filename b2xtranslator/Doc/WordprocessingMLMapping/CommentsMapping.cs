using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class CommentsMapping : DocumentMapping
{
    public CommentsMapping(ConversionContext ctx)
        : base(ctx, ctx.Docx.MainDocumentPart.CommentsPart)
    {
        _ctx = ctx;
    }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        var index = 0;
        
        _writer.WriteStartElement("w", "comments", OpenXmlNamespaces.WordprocessingML);
        
        var cp = doc.FIB.ccpText + doc.FIB.ccpFtn + doc.FIB.ccpHdr;
        for (var i = 0; i < doc.AnnotationsReferencePlex.Elements.Count; i++)
        {
            _writer.WriteStartElement("w", "comment", OpenXmlNamespaces.WordprocessingML);
            
            var atrdPre10 = doc.AnnotationsReferencePlex.Elements[index];
            _writer.WriteAttributeString("w", "id", OpenXmlNamespaces.WordprocessingML, index.ToString());
            _writer.WriteAttributeString("w", "author", OpenXmlNamespaces.WordprocessingML, doc.AnnotationOwners[atrdPre10.AuthorIndex]);
            _writer.WriteAttributeString("w", "initials", OpenXmlNamespaces.WordprocessingML, atrdPre10.UserInitials);
            
            //ATRDpost10 is optional and not saved in all files
            if (doc.AnnotationReferenceExtraTable != null &&
                doc.AnnotationReferenceExtraTable.Count > index)
            {
                var atrdPost10 = doc.AnnotationReferenceExtraTable[index];
                atrdPost10.Date.Convert(new DateMapping(_writer));
            }
            
            cp = writeParagraph(cp);
            _writer.WriteEndElement();
            index++;
        }
        
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
}