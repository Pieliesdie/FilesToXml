using System.IO;
using System.Linq;
using System.Reflection;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class XlsxToXmlTest : TestBase
{
    [Fact]
    public void XlsxConverterTestReadSomeCustomDates()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx4.xlsx";

        var result = converter.ConvertByFile(path);
        var isdateValid = result.Elements("TABLE").ElementAt(10).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "2")?.Attribute("C2")?.Value == "1998-12-11T00:00:00";
        Assert.True(isdateValid);
    }

    [Fact]
    public void XlsxConverterTestNotNull()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx.xlsx";

        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void XlsxConvertToXmlNotNull()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx.xlsx";
        using var fs = File.OpenRead(path);
        var result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void XlsxConverterTestReadSomeData()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx2.xlsx";

        var result = converter.ConvertByFile(path).Elements("TABLE").ToList();
        var isFirstTableValid = result.ElementAt(0).Attribute("name")?.Value == "Инструкция"
            && result.ElementAt(0).Elements("R").First(x => x.Attribute("id")?.Value == "16").Attribute("C1")?.Value == "Если есть вопросы - пожалуйста, обращайтесь к Е. Гостевой (29-33)";
        var isSecondTableValid = result.ElementAt(1).Attribute("name")?.Value == "АСУ ТП"
            && result.ElementAt(1).Elements("R").First(x => x.Attribute("id")?.Value == "13").Attribute("C1")?.Value == "18021:Волги";
        var isThirdTableValid = result.ElementAt(2).Attribute("name")?.Value == "Исходные данные - выгрузка"
            && result.ElementAt(2).Elements("R").First(x => x.Attribute("id")?.Value == "494").Attribute("C1")?.Value == "17158:Волги";

        Assert.True(isFirstTableValid);
        Assert.True(isSecondTableValid);
        Assert.True(isThirdTableValid);
    }

    [Fact]
    public void XlsxConverterTestReadLongNumber()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx3.xlsx";

        var result = converter.ConvertByFile(path);
        var isNumberValid = result
            .Elements("TABLE")
            .ElementAt(0)
            .Elements("R")
            .FirstOrDefault(r => r.Attribute("id")?.Value == "25")?.Attribute("C145")?.Value == "-0.0019999999894935172";
        Assert.True(isNumberValid);
    }

    [Fact]
    public void XlsxConverterTestReadBoolean()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx3.xlsx";

        var result = converter.ConvertByFile(path);
        var isBoolValid = result
            .Elements("TABLE")
            .ElementAt(0)
            .Elements("R")
            .FirstOrDefault(r => r.Attribute("id")?.Value == "25")?.Attribute("C148")?.Value == "True";
        Assert.True(isBoolValid);
    }

    [Fact]
    public void XlsxConverterTestLastRowId()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + "/Files/xlsx2.xlsx";

        var result = converter.ConvertByFile(path).Elements("TABLE").ToList();
        var isFirstTableValid = result.ElementAt(0).Attribute("name")?.Value == "Инструкция"
            && result.ElementAt(0).Elements("R").Last().Attribute("id")?.Value == "16";
        var isSecondTableValid = result.ElementAt(1).Attribute("name")?.Value == "АСУ ТП"
            && result.ElementAt(1).Elements("R").Last().Attribute("id")?.Value == "17763";
        var isThirdTableValid = result.ElementAt(2).Attribute("name")?.Value == "Исходные данные - выгрузка"
            && result.ElementAt(2).Elements("R").Last().Attribute("id")?.Value == "10006";

        Assert.True(isFirstTableValid);
        Assert.True(isSecondTableValid);
        Assert.True(isThirdTableValid);
    }
}