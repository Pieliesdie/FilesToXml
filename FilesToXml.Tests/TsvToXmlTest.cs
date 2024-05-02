using System.IO;
using System.Linq;
using System.Text;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class TsvToXmlTest
{
    private readonly TsvToXml converter = new();
    
    [Fact]
    public void TsvToXmlByFileTestNotNull()
    {
        var path = "./Files/tsv.tsv";
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void TsvConvertToXmlNotNull()
    {
        var path = "./Files/tsv.tsv";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void TsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var path = "./Files/tsv.tsv";
        var result = converter.ConvertByFile(path);
        Assert.Equal("rice", result.Elements().First().Elements().First().Attribute("C6").Value);
    }
}