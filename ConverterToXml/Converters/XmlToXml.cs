using System;
using System.IO;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XmlToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            var srcDoc = XDocument.Load(stream);
            var root = new XStreamingElement("DATASET", rootContent,  srcDoc.Root);
            return root;
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
