using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests
{
    public class DocxToXmlTest
    {
        [Fact]
        public void DocxConverterTestNotNull()
        {
            DocxToXml converter = new DocxToXml();
            string path = "./Files/docx.docx";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void DocxConvertToXmlNotNull()
        {
            var converter = new DocxToXml();
            string path = "./Files/docx.docx";
            using var fs = File.OpenRead(path);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
