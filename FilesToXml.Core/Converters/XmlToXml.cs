using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;

namespace FilesToXml.Core.Converters;

public class XmlToXml : IEncodingConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return Convert(stream, Encoding.UTF8, rootContent);
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        return ConvertByFile(path, Encoding.UTF8, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        return new XStreamingElement("DATASET", rootContent, Read(stream, encoding));
    }
    
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new XElement(Convert(fs, encoding, rootContent));
    }
    
    private static IEnumerable<object> Read(Stream stream, Encoding encoding)
    {
        using var sr = new StreamReader(stream, encoding);
        using var reader = XmlReader.Create(sr);
        foreach (var obj in ParseXml(reader))
        {
            yield return obj;
        }
    }
    
    private static IEnumerable<object> ParseXml(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    var rd = reader.ReadSubtree();
                    rd.MoveToContent();
                    yield return new XStreamingElement(reader.Name, ReadAttributes(reader).ToList(), ParseXml(rd));
                    break;
                case XmlNodeType.Text:
                    yield return new XText(reader.Value);
                    break;
                case XmlNodeType.CDATA:
                    yield return new XCData(reader.Value);
                    break;
                case XmlNodeType.Comment:
                    yield return new XComment(reader.Value);
                    break;
                default:
                    continue;
            }
        }
    }
    
    private static IEnumerable<XAttribute> ReadAttributes(XmlReader reader)
    {
        if (!reader.MoveToFirstAttribute())
        {
            yield break;
        }
        
        do
        {
            yield return new XAttribute(reader.Name, reader.Value);
        } while (reader.MoveToNextAttribute());
        
        reader.MoveToElement();
    }
}