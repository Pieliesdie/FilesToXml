using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class DocxToXmlTest : TestBase
{
    [Fact]
    public void DocxConverterTestNotNull()
    {
        var converter = new DocxToXml();
        var path = "./Files/docx.docx";
        
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void DocxConvertToXmlNotNull()
    {
        var converter = new DocxToXml();
        var path = "./Files/docx.docx";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
}