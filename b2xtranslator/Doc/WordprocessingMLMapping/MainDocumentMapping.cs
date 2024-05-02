using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class MainDocumentMapping : DocumentMapping
{
    public MainDocumentMapping(ConversionContext ctx, ContentPart targetPart)
        : base(ctx, targetPart) { }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        
        //start the document
        _writer.WriteStartDocument();
        _writer.WriteStartElement("w", "document", OpenXmlNamespaces.WordprocessingML);
        
        //write namespaces
        _writer.WriteAttributeString("xmlns", "v", null, OpenXmlNamespaces.VectorML);
        _writer.WriteAttributeString("xmlns", "o", null, OpenXmlNamespaces.Office);
        _writer.WriteAttributeString("xmlns", "w10", null, OpenXmlNamespaces.OfficeWord);
        _writer.WriteAttributeString("xmlns", "r", null, OpenXmlNamespaces.Relationships);
        _writer.WriteAttributeString("xmlns", "w14", null, OpenXmlNamespaces.WordprocessingML2010);
        
        _writer.WriteStartElement("w", "body", OpenXmlNamespaces.WordprocessingML);
        
        //convert the document
        _lastValidPapx = _doc.AllPapxFkps[0].grppapx[0];
        var cp = 0;
        while (cp < doc.FIB.ccpText)
        {
            var fc = _doc.PieceTable.FileCharacterPositions[cp];
            var papx = findValidPapx(fc);
            var tai = new TableInfo(papx);
            
            if (tai.fInTable)
                //this PAPX is for a table
            {
                cp = writeTable(cp, tai.iTap);
            }
            else
                //this PAPX is for a normal paragraph
            {
                cp = writeParagraph(cp);
            }
        }
        
        //write the section properties of the body with the last SEPX
        var lastSepxCp = 0;
        foreach (var sepxCp in _doc.AllSepx.Keys)
        {
            lastSepxCp = sepxCp;
        }
        
        var lastSepx = _doc.AllSepx[lastSepxCp];
        lastSepx.Convert(new SectionPropertiesMapping(_writer, _ctx, _sectionNr));
        
        //end the document
        _writer.WriteEndElement();
        _writer.WriteEndElement();
        _writer.WriteEndDocument();
        
        _writer.Flush();
    }
}