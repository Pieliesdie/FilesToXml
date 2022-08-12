using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class TxtToXml : IEncodingConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent) => this.Convert(stream, Encoding.UTF8, rootContent);

        public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
        {
            var sr = new StreamReader(stream, encoding);
            return new XStreamingElement("DATASET", rootContent, new XStreamingElement("TEXT", sr.ReadAllLines().Select(x=> $"{x}\r\n")));
        }

        public XElement ConvertByFile(string path, params object?[] rootContent) => this.ConvertByFile(path, Encoding.UTF8, rootContent);

        public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using FileStream fs = File.OpenRead(path);
            return new XElement(Convert(fs, encoding, rootContent));
        }
    }
}
