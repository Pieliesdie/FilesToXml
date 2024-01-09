using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Test;

public class CsvToXmlTest
{
    [Fact]
    public void CsvToXmlByFileTestNotNull()
    {
        var converter = new CsvToXml();
        string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/csv.csv";

        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void CsvConvertToXmlNotNull()
    {
        var converter = new CsvToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"\Files\csv.csv";
        using var fs = File.OpenRead(path);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void CsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var converter = new CsvToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/csv.csv";

        var result = converter.ConvertByFile(path, encoding: Encoding.GetEncoding(1251) );
        Assert.Equal("первый", Enumerable.First<XElement>(result.Elements()).Elements().First().Attribute("C1").Value);
    }
    
    [Fact]
    public void CsvToXmlTestAutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var converter = new CsvToXml();
        string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/csv2.csv";

        var result = converter.ConvertByFile(path, new[] {';', '|', '\t', ','} ,encoding: Encoding.GetEncoding(1251) );
        Assert.Equal("step_id", Enumerable.First<XElement>(result.Elements()).Elements().First().Attribute("C1").Value);
    }
    
    [Fact]
    public void CsvToXmlTest2AutoDetectDelimiter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var converter = new CsvToXml();
        string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/csv2.csv";

        var result = converter.ConvertByFile(path, new[] {';', '|', '\t', ','}, encoding: Encoding.GetEncoding(1251) );
        Assert.Equal("18526", Enumerable.First<XElement>(result.Elements()).Elements("R").Last().Attribute("C4").Value);
    }
}