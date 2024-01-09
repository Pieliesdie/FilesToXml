using System.IO;
using ConverterToXml.Core.Converters;
using Xunit;

namespace ConverterToXml.Test
{
    [Collection("XlsToXml")]
    public class XlsToXmlTest
    {
        [Fact]
        public void XlsConvertToXmlNotNull()
        {
            var converter = new XlsToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\xls.xls";
            using var fs = File.Open(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
