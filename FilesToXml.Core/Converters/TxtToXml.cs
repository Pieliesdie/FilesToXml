using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Extensions;

namespace FilesToXml.Core.Converters;

public class TxtToXml : IEncodingConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return Convert(stream, Encoding.UTF8, rootContent);
    }
    
    public XStreamingElement Convert(Stream stream, Encoding encoding, params object?[] rootContent)
    {
        return new XStreamingElement(DefaultStructure.DatasetName, rootContent,
            new XStreamingElement("TEXT", stream.ReadAllLinesWithNewLine(encoding)));
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        return ConvertByFile(path, Encoding.UTF8, rootContent);
    }
    
    public XElement ConvertByFile(string path, Encoding encoding, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, encoding, rootContent));
    }
}