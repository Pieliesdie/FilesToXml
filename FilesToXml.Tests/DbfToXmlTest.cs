using System.IO;
using System.Linq;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class DbfToXmlTest
{
    [Fact]
    public void DbfToXmlByFileTestNotNull()
    {
        var converter = new DbfToXml();
        string path = "./Files/dbf.dbf";

        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void DbfConvertToXmlNotNull()
    {
        var converter = new DbfToXml();
        string path = "./Files/dbf.dbf";
        using var fs = File.OpenRead(path);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void DbfToXmlTestReadFirstLine()
    {
        var converter = new DbfToXml();
        string path = "./Files/dbf.dbf";

        var result = converter.ConvertByFile(path);
        Assert.Equal("LSHET", Enumerable.First(result.Elements()).Elements().First().Attribute("C1").Value);
    }

    [Fact]
    public void DbfToXmlTestReadLastLine()
    {
        var converter = new DbfToXml();
        string path = "./Files/dbf.dbf";

        var result = converter.ConvertByFile(path);
        Assert.Equal("Ашарина", Enumerable.First(result.Elements()).Elements("R").Last().Attribute("C2").Value);
    }
}
