using System.IO;
using System.Reflection;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

[Collection("XlsToXml")]
public class XlsToXmlTest
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
}