using System.IO;
using System.Linq;
using ConverterToXml.Core.Converters;
using Xunit;

namespace ConverterToXml.Test;

public class DbfToXmlTest
{
    [Fact]
    public void DbfToXmlByFileTestNotNull()
    {
        var converter = new DbfToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/dbf.dbf";

        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void DbfConvertToXmlNotNull()
    {
        var converter = new DbfToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/dbf.dbf";
        using var fs = File.Open(path, FileMode.Open);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void DbfToXmlTestReadFirstLine()
    {
        var converter = new DbfToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/dbf.dbf";

        var result = converter.ConvertByFile(path);
        Assert.Equal("LSHET", result.Elements().First().Elements().First().Attribute("C1").Value);
    }

    [Fact]
    public void DbfToXmlTestReadLastLine()
    {
        var converter = new DbfToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/dbf.dbf";

        var result = converter.ConvertByFile(path);
        Assert.Equal("Ашарина", result.Elements().First().Elements("R").Last().Attribute("C2").Value);
    }
}
