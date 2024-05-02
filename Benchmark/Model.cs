using System.Reflection;
using BenchmarkDotNet.Attributes;
using FilesToXml.Core.Converters;

namespace Benchmark;

public class Model
{
    [Benchmark]
    public void smallxlsx()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xlsx.xlsx";
        
        using var fs = new FileStream(path, FileMode.Open);
        var result = converter.Convert(fs);
        _ = result?.ToString();
    }
    
    [Benchmark]
    public void bigxlsx()
    {
        var converter = new XlsxToXml();
        var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = curDir + @"/Files/xlsx2.xlsx";
        
        using var fs = new FileStream(path, FileMode.Open);
        var result = converter.Convert(fs);
        _ = result?.ToString();
    }
}