using ConverterToXml.Converters;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;

namespace ConverterToXml.Test
{
    public class JsonToXmlTest
    {
        [Fact]
        public void JsonToXmlTestNotNull()
        {
            var converter = new JsonToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/json.json";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void JsonToXmlConvertTestNotNull()
        {
            var converter = new JsonToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/json.json";
            using var fs = File.Open(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void JsonToXmlConvertRandomRead()
        {
            var converter = new JsonToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/json.json";
            using var fs = File.Open(path, FileMode.Open);
            var ds = new XElement(converter.Convert(fs));
            var result = ds.Element("ROOT").Element("items").Element("address").Element("region").Attribute("guid").Value;
            Assert.Equal("5f77834b-6351-4bcd-95af-a9ff7f6b511c", result);
        }
    }
}
