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
        public XDocument Convert(Stream stream, Encoding encoding);

        public XDocument ConvertByFile(string path, Encoding encoding);
    }
}
