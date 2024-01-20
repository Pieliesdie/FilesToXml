using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Extensions;

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
        return new XStreamingElement("DATASET", rootContent, ParseXML(XmlReader.Create(new StreamReader(stream, encoding))));
    }
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new XElement(Convert(fs, encoding, rootContent));
    }
    private static IEnumerable<object> ParseXML(XmlReader reader)
    {
        while (reader.Read())
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    var rd = reader.ReadSubtree();
                    rd.MoveToContent();
                    yield return new XStreamingElement(reader.Name, ReadAttributes(reader).ToList(), ParseXML(rd));
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
                case XmlNodeType.Whitespace:
                    yield return reader.Value;
                    break;
                case XmlNodeType.SignificantWhitespace:
                    yield return reader.Value;
                    break;
            }
    }
    private static IEnumerable<XAttribute> ReadAttributes(XmlReader reader)
    {
        if (reader.MoveToFirstAttribute())
        {
            do
            {
                yield return new XAttribute(reader.Name, reader.Value);
            } while (reader.MoveToNextAttribute());

            reader.MoveToElement();
        }
    }
}