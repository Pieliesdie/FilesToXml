using System.Text;
using System.Xml.Linq;

namespace FilesToXml.Core.Converters.Interfaces;

public interface IEncodingConvertable : IConvertable
{
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent);
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent);
}