using System.IO;
using System.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class DbfToXmlTest
{
    [Fact]
    public void DbfToXmlByFileTestNotNull()
    {
        var converter = new DbfToXml();
        var path = "./Files/dbf.dbf";
        
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void DbfConvertToXmlNotNull()
    {
        var converter = new DbfToXml();
        var path = "./Files/dbf.dbf";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void DbfToXmlTestReadFirstLine()
    {
        var converter = new DbfToXml();
        var path = "./Files/dbf.dbf";
        
        var result = converter.ConvertByFile(path);
        Assert.Equal("LSHET", result.Elements().First().Elements().First().Attribute("C1").Value);
    }
    
    [Fact]
    public void DbfToXmlTestReadLastLine()
    {
        var converter = new DbfToXml();
        var path = "./Files/dbf.dbf";
        
        var result = converter.ConvertByFile(path);
        Assert.Equal("Ашарина", result.Elements().First().Elements("R").Last().Attribute("C2").Value);
    }
}