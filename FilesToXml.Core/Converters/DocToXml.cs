using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Converters.OfficeConverters;

namespace FilesToXml.Core.Converters;

public class DocToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        var ms = DocToDocx.ConvertFromStreamToDocxMemoryStream(stream);
        ms.Position = 0;
        return new DocxToXml().Convert(ms, rootContent);
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return new XElement(Convert(fs, rootContent));
    }
}