using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Test
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
            using var fs = File.OpenRead(path);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
