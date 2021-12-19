using ConverterToXml.Converters;
using System.IO;
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

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string result = converter.Convert(fs).ToString();
                Assert.NotNull(result);
            }
        }
    }
}
