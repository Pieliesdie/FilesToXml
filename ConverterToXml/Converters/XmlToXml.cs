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
            return new XStreamingElement("DATASET", rootContent, XElement.Load(stream));
        }

        //static IEnumerable<XStreamingElement> ParseXML(Stream xmlContent)
        //{
        //    var reader = XmlReader.Create(xmlContent);
        //    var rootNodes = new List<XStreamingElement>();
        //    var nodeStack = new Stack<XStreamingElement>();
        //    while (reader.Read())
        //    {
        //        switch (reader.NodeType)
        //        {
        //            case XmlNodeType.Element:
        //                var node = new XStreamingElement(reader.Name);
        //                if (reader.MoveToFirstAttribute())
        //                {
        //                    // Read the attributes
        //                    do
        //                    {
        //                        node.Add(new XAttribute(reader.Name, reader.Value));
        //                    }
        //                    while (reader.MoveToNextAttribute());
        //                    // Move back to element
        //                    reader.MoveToElement();
        //                }
        //                if (nodeStack.Count > 0)
        //                {
        //                    nodeStack.Peek().Add(node);
        //                }
        //                else
        //                {
        //                    rootNodes.Add(node);
        //                }
        //                if (!reader.IsEmptyElement)
        //                    nodeStack.Push(node);
        //                break;

        //            case XmlNodeType.EndElement:
        //                nodeStack.Pop();
        //                break;
        //        }
        //    }
        //    return rootNodes;
        //}

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
