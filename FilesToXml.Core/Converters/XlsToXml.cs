using System.IO;
using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Converters.OfficeConverters;

namespace FilesToXml.Core.Converters;
public class XlsToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        var str = new XlsToXlsx().Convert(stream);
        var ms = new MemoryStream(str.ToArray());
        return new XlsxToXml().Convert(ms, rootContent);
    }

    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        path = path.RelativePathToAbsoluteIfNeed();
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }
}
