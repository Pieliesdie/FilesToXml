using System.IO;
using System.Reflection;
using System.Xml.XPath;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class XmlToXmlTest : TestBase
{
    [Fact]
    public void XmlToXmlTestNotNull()
    {
        var converter = new XmlToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xml.xml";
        
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void XmlToXmlConvertTestNotNull()
    {
        var converter = new XmlToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xml.xml";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void XmlToXmlTestReadSomeData()
    {
        var converter = new XmlToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xml.xml";
        var result = converter.ConvertByFile(path);
        var testElement = result.XPathSelectElement("/DATA/RES/HEADER/ReportDate");
        Assert.Equal("30.06.2021", testElement?.Value);
    }
}