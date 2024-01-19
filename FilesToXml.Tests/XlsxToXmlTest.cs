using System.IO;
using System.Linq;
using System.Xml.Linq;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Test
{
    public class XlsxToXmlTest
    {
        [Fact]
        public void XlsxConverterTestReadSomeCustomDates()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx4.xlsx";

            var result = converter.ConvertByFile(path);
            var isdateValid = result.Elements("TABLE").ElementAt(10).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "2")?.Attribute("C2")?.Value == "1998-12-11T00:00:00";
            Assert.True(isdateValid);
        }

        [Fact]
        public void XlsxConverterTestNotNull()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx.xlsx";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void XlsxConvertToXmlNotNull()
        {
            var converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx.xlsx";
            using var fs = File.OpenRead(path);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void XlsxConverterTestReadSomeData()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx2.xlsx";

            var result = converter.ConvertByFile(path).Elements("TABLE");
            var isFirstTableValid = result.ElementAt(0).Attribute("name").Value == "Инструкция"
                && result.ElementAt(0).Elements("R").Where(x => x.Attribute("id").Value == "16").First().Attribute("C1").Value == "Если есть вопросы - пожалуйста, обращайтесь к Е. Гостевой (29-33)";
            var isSecondTableValid = result.ElementAt(1).Attribute("name").Value == "АСУ ТП"
                && result.ElementAt(1).Elements("R").Where(x => x.Attribute("id").Value == "13").First().Attribute("C1").Value == "18021:Волги";
            var isThirdTableValid = result.ElementAt(2).Attribute("name").Value == "Исходные данные - выгрузка"
                && result.ElementAt(2).Elements("R").First(x => x.Attribute("id").Value == "494").Attribute("C1").Value == "17158:Волги";

            Assert.True(isFirstTableValid && isSecondTableValid && isThirdTableValid);
        }

        [Fact]
        public void XlsxConverterTestReadLongNumber()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx3.xlsx";

            var result = converter.ConvertByFile(path);
            var isNumberValid = result.Elements("TABLE").ElementAt(0).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "25")?.Attribute("C145")?.Value == "-0.0019999999894935172";
            Assert.True(isNumberValid);
        }

        [Fact]
        public void XlsxConverterTestReadBoolean()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx3.xlsx";


            var result = converter.ConvertByFile(path);
            var isBoolValid = result.Elements("TABLE").ElementAt(0).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "25")?.Attribute("C148")?.Value == "True";
            Assert.True(isBoolValid);
        }

        [Fact]
        public void XlsxConverterTestLastRowId()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx2.xlsx";

            var result = converter.ConvertByFile(path).Elements("TABLE");
            var isFirstTableValid = result.ElementAt(0).Attribute("name").Value == "Инструкция"
                && result.ElementAt(0).Elements("R").Last().Attribute("id").Value == "16";
            var isSecondTableValid = result.ElementAt(1).Attribute("name").Value == "АСУ ТП"
                && result.ElementAt(1).Elements("R").Last().Attribute("id").Value == "17763";
            var isThirdTableValid = result.ElementAt(2).Attribute("name").Value == "Исходные данные - выгрузка"
                && result.ElementAt(2).Elements("R").Last().Attribute("id").Value == "10006";

            Assert.True(isFirstTableValid && isSecondTableValid && isThirdTableValid);
        }
    }
}
