using System.IO;
using System.Linq;
using System.Text;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class CsvToXmlTest : TestBase
{
    private readonly CsvToXml converter = new();

    [Fact]
    public void CsvToXmlByFileTestNotNull()
    {
        var path = "./Files/csv.csv";
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void CsvConvertToXmlNotNull()
    {
        var path = "./Files/csv.csv";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void CsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var path = "./Files/csv.csv";
        var result = converter.ConvertByFile(path, Encoding.GetEncoding(1251));
        Assert.Equal("первый", result.Elements().First().Elements().First().Attribute("C1").Value);
    }

    [Fact]
    public void CsvToXmlTestAutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var path = "./Files/csv2.csv";
        var result = converter.ConvertByFile(path, new[] { ';', '|', '\t', ',' }, Encoding.GetEncoding(1251));
        Assert.Equal("step_id", result.Elements().First().Elements().First().Attribute("C1").Value);
    }

    [Fact]
    public void CsvToXmlTest2AutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var path = "./Files/csv2.csv";
        var result = converter.ConvertByFile(path, new[] { ';', '|', '\t', ',' }, Encoding.GetEncoding(1251));
        Assert.Equal("18526", result.Elements().First().Elements("R").Last().Attribute("C4").Value);
    }
}