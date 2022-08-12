using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public interface IEncodingConvertable : IConvertable
    {
        public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent);

        public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent);
    }
}
