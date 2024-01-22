using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;
public class TsvToXmlTest
{
    TsvToXml converter = new();
    [Fact]
    public void TsvToXmlByFileTestNotNull()
    {
        string path = "./Files/tsv.tsv";
        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TsvConvertToXmlNotNull()
    {
        string path = "./Files/tsv.tsv";
        using var fs = File.OpenRead(path);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string path = "./Files/tsv.tsv";
        var result = converter.ConvertByFile(path);
        Assert.Equal("rice", Enumerable.First(result.Elements()).Elements().First().Attribute("C6").Value);
    }

}