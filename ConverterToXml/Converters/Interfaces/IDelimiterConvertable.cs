using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Converters
{
    public interface IDelimiterConvertable : IConvertable, IEncodingConvertable
    {
        public XElement Convert(Stream stream, char[] searchingDelimiters, Encoding encoding);

        public XElement Convert(Stream stream, string delimiter, Encoding encoding);

        public XElement Convert(Stream stream, string delimiter);
        public XElement ConvertByFile(string path, char[] searchingDelimiters, Encoding encoding);

        public XElement ConvertByFile(string path, string delimiter, Encoding encoding);

        public XElement ConvertByFile(string path, string delimiter);
    }
}