using System.Xml.Linq;
using FilesToXml.Core.Converters.Interfaces;
using FilesToXml.Core.Converters.OfficeConverters;
using FilesToXml.Core.Defaults;

namespace FilesToXml.Core.Converters;

public class XlsToXml : IConvertable
{
    public XStreamingElement Convert(Stream stream, params object?[] rootContent)
    {
        return new XStreamingElement(DefaultStructure.DatasetName, rootContent, Read(stream));
    }
    
    public XElement ConvertByFile(string path, params object?[] rootContent)
    {
        using var fs = File.OpenRead(path);
        return new XElement(Convert(fs, rootContent));
    }
    
    private static IEnumerable<object> Read(Stream stream)
    {
        using var xlsx = XlsToXlsx.Convert(stream);
        xlsx.Position = 0;
        foreach (var obj in XlsxToXml.SpreadsheetProcess(xlsx))
        {
            yield return obj;
        }
    }
}