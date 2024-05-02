using System.Xml;

namespace b2xtranslator.CommonTranslatorLib;

public abstract class AbstractOpenXmlMapping
{
    protected XmlDocument _nodeFactory;
    protected XmlWriter _writer;
    
    public AbstractOpenXmlMapping(XmlWriter writer)
    {
        _writer = writer;
        _nodeFactory = new XmlDocument();
    }
}