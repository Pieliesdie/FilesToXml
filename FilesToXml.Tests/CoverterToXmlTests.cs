using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using FilesToXml.Console;
using FilesToXml.Core;
using Xunit;

namespace FilesToXml.Tests;

[CollectionDefinition("CoverterToXmlTests", DisableParallelization = true)]
public class CoverterToXmlTests : TestBase
{
    [Fact]
    public void Convert_WithOptionsAndStreams_WritesToFile()
    {
        var options = new Options
        {
            Input = ["./Files/csv.csv"],
            Output = "output.xml",
            ForceSave = true,
            OutputEncoding = Encoding.UTF8.CodePage
        };
        
        using var outputStream = new MemoryStream();
        using var errorStream = new MemoryStream();
        
        // Act
        var success = ConverterToXml.Convert(options.MapToIOptions(), outputStream, errorStream);
        
        // Assert
        Assert.True(success);
        
        using var fs = File.OpenRead("output.xml");
        using var sr = new StreamReader(fs);
        var outputContent = sr.ReadToEnd();
        Assert.NotEmpty(outputContent);
        
        using var errorSr = new StreamReader(errorStream);
        var errorContent = errorSr.ReadToEnd();
        Assert.Empty(errorContent);
    }
    
    [Fact]
    public void ReadCsvFromStream()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var options = new Options
        {
            Input = ["./Files/csv.csv"],
            InputEncoding = [1251],
            OutputEncoding = Encoding.UTF8.CodePage
        };
        
        using var outputStream = new MemoryStream();
        using var errorStream = new MemoryStream();
        
        var success = ConverterToXml.Convert(options.MapToIOptions(), outputStream, errorStream);
        
        Assert.True(success);
        
        var outputContent = new StreamReader(outputStream).ReadToEnd();
        Assert.NotEmpty(outputContent);
        
        var errorContent = new StreamReader(errorStream).ReadToEnd();
        Assert.Empty(errorContent);
        
        var cellValue = XDocument.Parse(outputContent)
            .XPathSelectElement("/DATA/DATASET/TABLE/R[@id=79]")?
            .Attribute("C5")?.Value;
        Assert.Equal("апрель", cellValue);
    }
    
    [Fact]
    public void MultipleXlsx()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var options = new Options
        {
            Input = ["./Files/xlsx.xlsx", "./Files/xlsx2.xlsx"],
            InputEncoding = [1251, 65001],
            OutputEncoding = Encoding.UTF8.CodePage,
            ForceSave = true,
            Labels = ["test1", "test2"]
        };
        
        using var outputStream = new MemoryStream();
        using var errorStream = new MemoryStream();
        
        var success = ConverterToXml.Convert(options.MapToIOptions(), outputStream, errorStream);
        
        Assert.True(success);
        
        var outputContent = new StreamReader(outputStream).ReadToEnd();
        Assert.NotEmpty(outputContent);
        
        var errorContent = new StreamReader(errorStream).ReadToEnd();
        Assert.Empty(errorContent);
        
        var xdoc = XDocument.Parse(outputContent);
        
        Assert.Equal(2, xdoc.XPathSelectElements("/DATA/DATASET").Count());
        
        var cellValue = xdoc
            .XPathSelectElement("/DATA/DATASET[@label='test1']/TABLE/R[@id=79]")?
            .Attribute("C5")?.Value;
        Assert.Equal("апрель", cellValue);
    }
}