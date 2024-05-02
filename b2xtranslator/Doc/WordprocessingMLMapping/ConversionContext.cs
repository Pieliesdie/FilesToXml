using System.Collections.Generic;
using System.Xml;
using b2xtranslator.doc.DocFileFormat;
using b2xtranslator.OpenXmlLib.WordprocessingML;

namespace b2xtranslator.doc.WordprocessingMLMapping;

public class ConversionContext
{
    //private Dictionary<Int32, SectionPropertyExceptions> _allSepx;
    //private Dictionary<Int32, ParagraphPropertyExceptions> _allPapx;
    /// <summary>
    ///     A list thta contains all revision ids.
    /// </summary>
    public List<string> AllRsids;
    
    public ConversionContext(WordDocument doc)
    {
        Doc = doc;
        AllRsids = new List<string>();
    }
    
    /// <summary>
    ///     The source of the conversion.
    /// </summary>
    public WordDocument Doc { get; set; }
    
    /// <summary>
    ///     This is the target of the conversion.<br />
    ///     The result will be written to the parts of this document.
    /// </summary>
    public WordprocessingDocument Docx { get; set; }
    
    /// <summary>
    ///     The settings of the XmlWriter which writes to the part
    /// </summary>
    public XmlWriterSettings WriterSettings { get; set; }
    
    /// <summary>
    ///     Adds a new RSID to the list
    /// </summary>
    /// <param name="rsid"></param>
    public void AddRsid(string rsid)
    {
        if (!AllRsids.Contains(rsid))
        {
            AllRsids.Add(rsid);
        }
    }
}