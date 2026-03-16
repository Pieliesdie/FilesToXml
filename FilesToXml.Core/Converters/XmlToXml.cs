using System.Text;
using System.Xml;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;

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
        return new XStreamingElement(DefaultStructure.DatasetName, rootContent, Read(stream, encoding));
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

    private static IEnumerable<object> ParseXml(XmlReader reader, string? inheritedDefaultNs = null)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    var defaultNs = reader.GetAttribute("xmlns") ?? inheritedDefaultNs;
                    XName name = string.IsNullOrEmpty(reader.NamespaceURI) || reader.NamespaceURI == defaultNs
                        ? (XName)reader.LocalName
                        : (XNamespace)reader.NamespaceURI + reader.LocalName;
                    var rd = reader.ReadSubtree();
                    rd.MoveToContent();
                    yield return new XStreamingElement(name, ReadAttributes(reader, defaultNs).ToList(), ParseXml(rd, defaultNs));
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
            }
        }
    }

    private static IEnumerable<XAttribute> ReadAttributes(XmlReader reader, string? defaultNs = null)
    {
        if (!reader.MoveToFirstAttribute())
            yield break;

        do
        {
            if (reader.Name == "xmlns")
            {
                if (reader.Value != defaultNs)
                    yield return new XAttribute("xmlns", reader.Value);
            }
            else if (reader.Prefix == "xmlns")
            {
                if (reader.Value == defaultNs)
                    continue;

                yield return new XAttribute(XNamespace.Xmlns + reader.LocalName, reader.Value);
            }
            else
            {
                if (string.IsNullOrEmpty(reader.NamespaceURI) || reader.NamespaceURI == defaultNs)
                    yield return new XAttribute(reader.LocalName, reader.Value);
                else
                    yield return new XAttribute((XNamespace)reader.NamespaceURI + reader.LocalName, reader.Value);
            }
        } while (reader.MoveToNextAttribute());

        reader.MoveToElement();
    }
}