using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Xml;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XmlToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            return new XStreamingElement("DATASET", rootContent, ParseXML(XmlReader.Create(stream)));
        }

        private static IEnumerable<XStreamingElement> ParseXML(XmlReader reader)
        {
            while (reader.Read())
            { 
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        var rd = reader.ReadSubtree();
                        rd.MoveToContent();
                        yield return new XStreamingElement(reader.Name, ReadAttributes(reader).ToList(), ParseXML(rd));
                        break;
                }
            }
        }

        private static IEnumerable<XAttribute> ReadAttributes(XmlReader reader)
        {
            if (reader.MoveToFirstAttribute())
            {
                // Read the attributes
                do
                {
                    yield return new XAttribute(reader.Name, reader.Value);
                }
                while (reader.MoveToNextAttribute());
                // Move back to element
                reader.MoveToElement();
            }
        }

        public XElement ConvertByFile(string path, params object?[] rootContent)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new XElement(Convert(fs, rootContent));
        }
    }
}
