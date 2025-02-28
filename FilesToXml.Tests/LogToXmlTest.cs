using System.IO;
using System.Linq;
using System.Text;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class LogToXmlTest : TestBase
{
    private readonly LogToXml converter = new();
    
    [Fact]
    public void LogToXmlByFileTestNotNull()
    {
        var path = "./Files/log.log";
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void LogConvertToXmlNotNull()
    {
        var path = "./Files/log.log";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void LogToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var path = "./Files/log.log";
        var result = converter.ConvertByFile(path);
        Assert.Equal("5", result.Elements().First().Elements().First().Attribute("C2").Value);
    }
}