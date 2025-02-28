using System.IO;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class JsonToXmlTest : TestBase
{
    [Fact]
    public void JsonToXmlTestNotNull()
    {
        var converter = new JsonToXml();
        var path = "./Files/json.json";
        
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void JsonToXmlConvertTestNotNull()
    {
        var converter = new JsonToXml();
        var path = "./Files/json.json";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void JsonToXmlConvertRandomRead()
    {
        var converter = new JsonToXml();
        var path = "./Files/json.json";
        using var fs = File.OpenRead(path);
        var ds = new XElement(converter.Convert(fs));
        var result = ds.Element("items").Element("address").Element("region").Attribute("guid").Value;
        Assert.Equal("5f77834b-6351-4bcd-95af-a9ff7f6b511c", result);
    }
}