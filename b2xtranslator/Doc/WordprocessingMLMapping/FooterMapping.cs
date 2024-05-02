using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.OpenXmlLib.WordprocessingML;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class FooterMapping : DocumentMapping
{
    private readonly CharacterRange _ftr;
    
    public FooterMapping(ConversionContext ctx, FooterPart part, CharacterRange ftr)
        : base(ctx, part)
    {
        _ftr = ftr;
    }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        
        _writer.WriteStartDocument();
        _writer.WriteStartElement("w", "ftr", OpenXmlNamespaces.WordprocessingML);
        
        //convert the footer text
        _lastValidPapx = _doc.AllPapxFkps[0].grppapx[0];
        var cp = _ftr.CharacterPosition;
        var cpMax = _ftr.CharacterPosition + _ftr.CharacterCount;
        
        //the CharacterCount of the footers also counts the guard paragraph mark.
        //this additional paragraph mark shall not be converted.
        cpMax--;
        
        while (cp < cpMax)
        {
            var fc = _doc.PieceTable.FileCharacterPositions[cp];
            var papx = findValidPapx(fc);
            var tai = new TableInfo(papx);
            
            if (tai.fInTable)
            {
                //this PAPX is for a table
                cp = writeTable(cp, tai.iTap);
            }
            else
            {
                //this PAPX is for a normal paragraph
                cp = writeParagraph(cp);
            }
        }
        
        _writer.WriteEndElement();
        _writer.WriteEndDocument();
        
        _writer.Flush();
    }
}