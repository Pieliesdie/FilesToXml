using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public interface IDelimiterConvertable : IConvertable, IEncodingConvertable
    {
        public XDocument Convert(Stream stream, string delimiter, Encoding encoding);

        public XDocument Convert(Stream stream, string delimiter);

        public XDocument ConvertByFile(string path, string delimiter, Encoding encoding);

        public XDocument ConvertByFile(string path, string delimiter);
    }
}
