using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class DocToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            DocToDocx docToDocx = new DocToDocx();
            MemoryStream ms = docToDocx.ConvertFromStreamToDocxMemoryStream(stream);
            var docxToXml = new DocxToXml();
            return (docxToXml.Convert(ms, rootContent));
        }

        public XElement ConvertByFile(string path, params object?[] rootContent)
        {
            path = path.RelativePathToAbsoluteIfNeed();
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new XElement(Convert(fs, rootContent));
        }
    }
}
