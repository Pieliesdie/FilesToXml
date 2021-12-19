using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XlsToXml : IConvertable
    {
        public XDocument Convert(Stream stream)
        {
            var str = new XlsToXlsx().Convert(stream);
            using var ms = new MemoryStream(str.ToArray());
            return new XlsxToXml().Convert(ms);
        }

        public XDocument ConvertByFile(string path)
        {
            using var fs = File.OpenRead(path);
            return Convert(fs);
        }
    }
}
