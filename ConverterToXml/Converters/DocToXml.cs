using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class DocToXml : IConvertable
    {
        public XDocument Convert(Stream stream)
        {
            DocToDocx docToDocx = new DocToDocx();
            MemoryStream ms = docToDocx.ConvertFromStreamToDocxMemoryStream(stream);
            var docxToXml = new DocxToXml();
            return (docxToXml.Convert(ms));
        }

        public XDocument ConvertByFile(string path)
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
