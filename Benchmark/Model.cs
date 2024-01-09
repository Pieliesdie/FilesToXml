using BenchmarkDotNet.Attributes;
using FilesToXml.Core.Converters;

namespace Benchmark
{
    public class Model
    {
        [Benchmark]
        public void smallxlsx()
        {
            XlsxToXml converter = new XlsxToXml();
            string? curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx.xlsx";

            using FileStream fs = new FileStream(path, FileMode.Open);
            var result = converter.Convert(fs);
            _ = result?.ToString();
        }

        [Benchmark]
        public void bigxlsx()
        {
            XlsxToXml converter = new XlsxToXml();
            string? curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx2.xlsx";

            using FileStream fs = new FileStream(path, FileMode.Open);
            var result = converter.Convert(fs);
            _ = result?.ToString();
        }
    }
}
