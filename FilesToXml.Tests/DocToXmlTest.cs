using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

[Collection("DocToXml")]
public class DocToXmlTest
{
    [Fact]
    public void DocToDocxConvertByFileToXmlNotNull()
    {
        var converter = new DocToXml();
        var path = "./Files/doc1.doc";
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void DocToDocxConvertToXmlNotNull()
    {
        var converter = new DocToXml();
        var path = "./Files/doc1.doc";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
}