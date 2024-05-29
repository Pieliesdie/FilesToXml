using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;

namespace FilesToXml.Core.Converters;

public class LogToXml : IEncodingConvertable
{
    private const string LogDelimiter = "|";
    
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        return new CsvToXml().Convert(stream, LogDelimiter, encoding, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return Convert(stream, Encoding.UTF8, rootContent);
    }
    
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        return new CsvToXml().ConvertByFile(path, LogDelimiter, encoding, rootContent);
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        return ConvertByFile(path, Encoding.UTF8, rootContent);
    }
}