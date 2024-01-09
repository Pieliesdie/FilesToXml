using System.IO;
using FilesToXml.Core.Converters.OfficeConverters;
using Xunit;

namespace FilesToXml.Test
{
    [Collection("XlsToXml")]
    public class XlsToXlsxTest
    {
        [Fact]
        public void XlsToXlsxConverterTestNotNull()
        {
            XlsToXlsx converter = new XlsToXlsx();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xls.xls";
            converter.ConvertToXlsxFile(path, curDir + @"/Files/Result.xlsx");
            Assert.True(File.Exists(curDir + @"/Files/Result.xlsx"));
        }
    }
}
