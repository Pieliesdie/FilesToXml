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
        public XElement Convert(Stream stream, Encoding encoding);

        public XElement ConvertByFile(string path, Encoding encoding);
    }
}
