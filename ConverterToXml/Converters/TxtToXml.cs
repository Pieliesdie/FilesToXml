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
        public XDocument Convert(Stream stream) => this.Convert(stream, Encoding.UTF8);

        public XDocument Convert(Stream stream, Encoding encoding)
        {
            using var sr = new StreamReader(stream, encoding);
            var root = new XElement("DATASET", new XElement("TEXT", sr.ReadToEnd()));
            return new XDocument(root);
        }

        public XDocument ConvertByFile(string path) => this.ConvertByFile(path, Encoding.UTF8);

        public XDocument ConvertByFile(string path, Encoding encoding)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Convert(fs, encoding);
        }
    }
}
