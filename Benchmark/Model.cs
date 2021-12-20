using BenchmarkDotNet.Attributes;
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
        public void smallxlsx() => xlsx.XlsxConverterTestNotNull();

        [Benchmark]
        public void bigxlsx() => xlsx.XlsxConverterTestReadSomeData();

    }
}
