using BenchmarkDotNet.Attributes;
using ConverterToXml.Converters;
using ConverterToXml.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public class Model
    {
        public XlsxToXmlTest xlsx = new XlsxToXmlTest();

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
