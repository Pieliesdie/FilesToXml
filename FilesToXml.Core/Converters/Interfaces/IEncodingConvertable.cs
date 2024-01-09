using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ConverterToXml.Core.Converters.Interfaces
{
    public interface IEncodingConvertable : IConvertable
    {
        public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent);

        public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent);
    }
}
