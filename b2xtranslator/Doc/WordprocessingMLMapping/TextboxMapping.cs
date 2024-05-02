using System.Xml;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;
using b2xtranslator.OpenXmlLib.WordprocessingML;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class TextboxMapping : DocumentMapping
{
    public static int TextboxCount;
    private readonly int _textboxIndex;
    
    public TextboxMapping(ConversionContext ctx, int textboxIndex, ContentPart targetpart, XmlWriter writer)
        : base(ctx, targetpart, writer)
    {
        TextboxCount++;
        _textboxIndex = textboxIndex;
    }
    
    public TextboxMapping(ConversionContext ctx, ContentPart targetpart, XmlWriter writer)
        : base(ctx, targetpart, writer)
    {
        TextboxCount++;
        _textboxIndex = TextboxCount - 1;
    }
    
    public override void Apply(WordDocument doc)
    {
        _doc = doc;
        
        _writer.WriteStartElement("v", "textbox", OpenXmlNamespaces.VectorML);
        _writer.WriteStartElement("w", "txbxContent", OpenXmlNamespaces.WordprocessingML);
        
        var cp = 0;
        var cpEnd = 0;
        BreakDescriptor bkd = null;
        var txtbxSubdocStart = doc.FIB.ccpText + doc.FIB.ccpFtn + doc.FIB.ccpHdr + doc.FIB.ccpAtn + doc.FIB.ccpEdn;
        
        if (_targetPart.GetType() == typeof(MainDocumentPart))
        {
            cp = txtbxSubdocStart + doc.TextboxBreakPlex.CharacterPositions[_textboxIndex];
            cpEnd = txtbxSubdocStart + doc.TextboxBreakPlex.CharacterPositions[_textboxIndex + 1];
            bkd = doc.TextboxBreakPlex.Elements[_textboxIndex];
        }
        
        if (_targetPart.GetType() == typeof(HeaderPart) || _targetPart.GetType() == typeof(FooterPart))
        {
            txtbxSubdocStart += doc.FIB.ccpTxbx;
            cp = txtbxSubdocStart + doc.TextboxBreakPlexHeader.CharacterPositions[_textboxIndex];
            cpEnd = txtbxSubdocStart + doc.TextboxBreakPlexHeader.CharacterPositions[_textboxIndex + 1];
            bkd = doc.TextboxBreakPlexHeader.Elements[_textboxIndex];
        }
        
        //convert the textbox text
        _lastValidPapx = _doc.AllPapxFkps[0].grppapx[0];
        
        while (cp < cpEnd)
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
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
}