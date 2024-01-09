using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConverterToXml.Core.Converters;
using Xunit;

namespace ConverterToXml.Test
{
    public class XmlToXmlTest
    {
        [Fact]
        public void XmlToXmlTestNotNull()
        {
            var converter = new XmlToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xml.xml";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void XmlToXmlConvertTestNotNull()
        {
            var converter = new XmlToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xml.xml";
            using var fs = File.Open(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
