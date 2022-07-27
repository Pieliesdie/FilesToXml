using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConverterToXml.Converters;
using Xunit;

namespace ConverterToXml.Test
{
    public class XlsxToXmlTest
    {
        [Fact]
        public void XlsxConverterTestNotNull()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx.xlsx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string result = converter.Convert(fs).ToString();
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void XlsxConverterTestReadSomeData()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx2.xlsx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var result = converter.Convert(fs);
                var isFirstTableValid = result.Elements("TABLE").ElementAt(0).Attribute("name").Value == "Инструкция"
                    && result.Elements("TABLE").ElementAt(0).Elements("R").Where(x => x.Attribute("id").Value == "16").First().Attribute("C1").Value == "Если есть вопросы - пожалуйста, обращайтесь к Е. Гостевой (29-33)";
                var isSecondTableValid = result.Elements("TABLE").ElementAt(1).Attribute("name").Value == "АСУ ТП"
                    && result.Elements("TABLE").ElementAt(1).Elements("R").Where(x => x.Attribute("id").Value == "13").First().Attribute("C1").Value == "18021:Волги";
                var isThirdTableValid = result.Elements("TABLE").ElementAt(2).Attribute("name").Value == "Исходные данные - выгрузка"
                    && result.Elements("TABLE").ElementAt(2).Elements("R").Where(x => x.Attribute("id").Value == "494").First().Attribute("C1").Value == "17158:Волги";

                Assert.True(isFirstTableValid && isSecondTableValid && isThirdTableValid);
            }
        }

        [Fact]
        public void XlsxConverterTestReadLongNumber()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx3.xlsx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var result = converter.Convert(fs);
                var isNumberValid = result.Elements("TABLE").ElementAt(0).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "25")?.Attribute("C145")?.Value == "-0.0019999999894935172";
                Assert.True(isNumberValid);
            }
        }

        [Fact]
        public void XlsxConverterTestReadBoolean()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx3.xlsx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var result = converter.Convert(fs);
                var isBoolValid = result.Elements("TABLE").ElementAt(0).Elements("R").FirstOrDefault(R => R.Attribute("id").Value == "25")?.Attribute("C148")?.Value == "True";
                Assert.True(isBoolValid);
            }
        }

        [Fact]
        public void XlsxConverterTestLastRowId()
        {
            XlsxToXml converter = new XlsxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xlsx2.xlsx";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                var result = converter.Convert(fs);
                var isFirstTableValid = result.Elements("TABLE").ElementAt(0).Attribute("name").Value == "Инструкция"
                    && result.Elements("TABLE").ElementAt(0).Elements("R").Last().Attribute("id").Value == "16";
                var isSecondTableValid = result.Elements("TABLE").ElementAt(1).Attribute("name").Value == "АСУ ТП"
                    && result.Elements("TABLE").ElementAt(1).Elements("R").Last().Attribute("id").Value == "17763";
                var isThirdTableValid = result.Elements("TABLE").ElementAt(2).Attribute("name").Value == "Исходные данные - выгрузка"
                    && result.Elements("TABLE").ElementAt(2).Elements("R").Last().Attribute("id").Value == "10006";

                Assert.True(isFirstTableValid && isSecondTableValid && isThirdTableValid);
            }
        }
    }
}
