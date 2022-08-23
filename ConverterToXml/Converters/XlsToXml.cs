using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public class XlsToXml : IConvertable
    {
        public XStreamingElement Convert(Stream stream, params object?[] rootContent)
        {
            var str = new XlsToXlsx().Convert(stream);
            var ms = new MemoryStream(str.ToArray());
            return new XlsxToXml().Convert(ms, rootContent);
        }

        public XElement ConvertByFile(string path, params object?[] rootContent)
        {
            path = path.RelativePathToAbsoluteIfNeed();
            using var fs = File.OpenRead(path);
            return new XElement(Convert(fs, rootContent));
        }
    }
}
