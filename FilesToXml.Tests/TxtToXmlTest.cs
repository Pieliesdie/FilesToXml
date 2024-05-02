using System.IO;
using System.Reflection;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests;

public class TxtToXmlTest
{
    [Fact]
    public void TxtToXmlTestNotNull()
    {
        var converter = new TxtToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"\Files\txt.txt";
        
        var result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void TxtConvertToXmlNotNull()
    {
        var converter = new TxtToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"\Files\txt.txt";
        var result = converter.Convert(File.OpenRead(path)).ToString();
        Assert.NotNull(result);
    }
}