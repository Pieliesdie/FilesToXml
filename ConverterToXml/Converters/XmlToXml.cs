using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XmlToXml : IConvertable
    {
        public XElement Convert(Stream stream)
        {
            var srcDoc = XDocument.Load(stream);
            var root = new XElement("DATASET", srcDoc.Root);
            return root;
        }

        public XElement ConvertByFile(string path)
        {
            if (!Path.IsPathFullyQualified(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Convert(fs);
        }
    }
}
