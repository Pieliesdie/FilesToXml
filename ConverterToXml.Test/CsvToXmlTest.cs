using ConverterToXml.Converters;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ConverterToXml.Test
{
    public class CsvToXmlTest
    {
        [Fact]
        public void CsvToXmlTestNotNull()
        {
            var converter = new CsvToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/csv.csv";

            using FileStream fs = new FileStream(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void CsvToXmlTestReadFirstLine()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var converter = new CsvToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/csv.csv";

            using FileStream fs = new FileStream(path, FileMode.Open);
            var result = converter.Convert(fs, encoding: Encoding.GetEncoding(1251) );
            Assert.Equal("первый", result.Root.Elements().First().Elements().First().Attribute("C1").Value);
        }
        
        [Fact]
        public void CsvToXmlTestAutoDetectDelimiter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var converter = new CsvToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/csv2.csv";

            using FileStream fs = new FileStream(path, FileMode.Open);
            var result = converter.Convert(fs, new[] {';', '|', '\t', ','} ,encoding: Encoding.GetEncoding(1251) );
            Assert.Equal("step_id", result.Root.Elements().First().Elements().First().Attribute("C1").Value);
        }
        
        [Fact]
        public void CsvToXmlTest2AutoDetectDelimiter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var converter = new CsvToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/csv2.csv";

            using FileStream fs = new FileStream(path, FileMode.Open);
            var result = converter.Convert(fs, new[] {';', '|', '\t', ','}, encoding: Encoding.GetEncoding(1251) );
            Assert.Equal("18526", result.Root.Elements().First().Elements().Last().Attribute("C4").Value);
        }
    }
}