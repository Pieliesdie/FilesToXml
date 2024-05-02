using System.IO;
using System.Reflection;
using FilesToXml.Core.Converters.OfficeConverters;
using Xunit;

namespace FilesToXml.Tests;

[Collection("XlsToXml")]
public class XlsToXlsxTest
{
    [Fact]
    public void XlsToXlsxConverterTestNotNull()
    {
        var converter = new XlsToXlsx();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xls.xls";
        converter.ConvertToXlsxFile(path, curDir + @"/Files/Result.xlsx");
        Assert.True(File.Exists(curDir + @"/Files/Result.xlsx"));
    }
}