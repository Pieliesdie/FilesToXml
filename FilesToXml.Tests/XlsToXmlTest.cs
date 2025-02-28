using System.IO;
using System.Linq;
using System.Reflection;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

[Collection("XlsToXml")]
public class XlsToXmlTest : TestBase
{
    [Fact]
    public void XlsConvertToXmlNotNull()
    {
        var converter = new XlsToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"\Files\xls.xls";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void XlsConvertTestReadSomeData()
    {
        var converter = new XlsToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xls.xls";
        
        var result = converter.ConvertByFile(path).Elements("TABLE").ToList();
        var isFirstTableValid = result.ElementAt(0).Attribute("name")?.Value == "Лист1"
            && result.ElementAt(0).Elements("R").First(x => x.Attribute("id")?.Value == "1").Attribute("C1")?.Value == "первый";
        var isSecondTableValid = result.ElementAt(1).Attribute("name")?.Value == "My parent's accounting"
            && result.ElementAt(1).Elements("R").First(x => x.Attribute("id")?.Value == "25").Attribute("C5")?.Value == "декабрь";
        
        Assert.True(isFirstTableValid);
        Assert.True(isSecondTableValid);
    }
}