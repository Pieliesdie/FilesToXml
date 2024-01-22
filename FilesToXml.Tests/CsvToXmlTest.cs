using System.IO;
using System.Linq;
using System.Text;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class CsvToXmlTest
{
    CsvToXml converter = new();
    [Fact]
    public void CsvToXmlByFileTestNotNull()
    {
        string path = "./Files/csv.csv";
        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    [Fact]
    public void CsvConvertToXmlNotNull()
    {
        string path = "./Files/csv.csv";
        using var fs = File.OpenRead(path);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    [Fact]
    public void CsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string path = "./Files/csv.csv";
        var result = converter.ConvertByFile(path, encoding: Encoding.GetEncoding(1251));
        Assert.Equal("первый", Enumerable.First(result.Elements()).Elements().First().Attribute("C1").Value);
    }
    [Fact]
    public void CsvToXmlTestAutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string path = "./Files/csv2.csv";
        var result = converter.ConvertByFile(path, new[] { ';', '|', '\t', ',' }, encoding: Encoding.GetEncoding(1251));
        Assert.Equal("step_id", Enumerable.First(result.Elements()).Elements().First().Attribute("C1").Value);
    }
    [Fact]
    public void CsvToXmlTest2AutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string path = "./Files/csv2.csv";
        var result = converter.ConvertByFile(path, new[] { ';', '|', '\t', ',' }, encoding: Encoding.GetEncoding(1251));
        Assert.Equal("18526", Enumerable.First(result.Elements()).Elements("R").Last().Attribute("C4").Value);
    }
}