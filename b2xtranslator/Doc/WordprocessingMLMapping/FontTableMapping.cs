using System.Xml;
using b2xtranslator.CommonTranslatorLib;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class FontTableMapping : AbstractOpenXmlMapping,
    IMapping<StringTable>
{
    public FontTableMapping(ConversionContext ctx, OpenXmlPart targetPart)
        : base(XmlWriter.Create(targetPart.GetStream(), ctx.WriterSettings)) { }
    
    public void Apply(StringTable table)
    {
        _writer.WriteStartElement("w", "fonts", OpenXmlNamespaces.WordprocessingML);
        
        foreach (FontFamilyName font in table.Data)
        {
            _writer.WriteStartElement("w", "font", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "name", OpenXmlNamespaces.WordprocessingML, font.xszFtn);
            
            //alternative name
            if (font.xszAlt != null && font.xszAlt.Length > 0)
            {
                _writer.WriteStartElement("w", "altName", OpenXmlNamespaces.WordprocessingML);
                _writer.WriteAttributeString("w", "val", OpenXmlNamespaces.WordprocessingML, font.xszAlt);
                _writer.WriteEndElement();
            }
            
            //charset
            _writer.WriteStartElement("w", "charset", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "val", OpenXmlNamespaces.WordprocessingML, $"{font.chs:x2}");
            _writer.WriteEndElement();
            
            //font family
            _writer.WriteStartElement("w", "family", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "val", OpenXmlNamespaces.WordprocessingML, ((FontFamily)font.ff).ToString());
            _writer.WriteEndElement();
            
            //panose
            _writer.WriteStartElement("w", "panose1", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteStartAttribute("w", "val", OpenXmlNamespaces.WordprocessingML);
            foreach (var b in font.panose)
            {
                _writer.WriteString($"{b:x2}");
            }
            
            _writer.WriteEndAttribute();
            _writer.WriteEndElement();
            
            //pitch
            _writer.WriteStartElement("w", "pitch", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "val", OpenXmlNamespaces.WordprocessingML, font.prq.ToString());
            _writer.WriteEndElement();
            
            //truetype
            if (!font.fTrueType)
            {
                _writer.WriteStartElement("w", "notTrueType", OpenXmlNamespaces.WordprocessingML);
                _writer.WriteAttributeString("w", "val", OpenXmlNamespaces.WordprocessingML, "true");
                _writer.WriteEndElement();
            }
            
            //font signature
            _writer.WriteStartElement("w", "sig", OpenXmlNamespaces.WordprocessingML);
            _writer.WriteAttributeString("w", "usb0", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.UnicodeSubsetBitfield0:x8}");
            _writer.WriteAttributeString("w", "usb1", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.UnicodeSubsetBitfield1:x8}");
            _writer.WriteAttributeString("w", "usb2", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.UnicodeSubsetBitfield2:x8}");
            _writer.WriteAttributeString("w", "usb3", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.UnicodeSubsetBitfield3:x8}");
            _writer.WriteAttributeString("w", "csb0", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.CodePageBitfield0:x8}");
            _writer.WriteAttributeString("w", "csb1", OpenXmlNamespaces.WordprocessingML,
                $"{font.fs.CodePageBitfield1:x8}");
            _writer.WriteEndElement();
            
            _writer.WriteEndElement();
        }
        
        _writer.WriteEndElement();
        
        _writer.Flush();
    }
    
    protected enum FontFamily
    {
        auto,
        decorative,
        modern,
        roman,
        script,
        swiss
    }
}