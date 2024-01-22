using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Tests
{
    [Collection("DocToXml")]
    public class DocToXmlTest
    {
        [Fact]
        public void DocToDocxConvertByFileToXmlNotNull()
        {
            DocToXml converter = new DocToXml();
            string path = "./Files/doc1.doc";
            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void DocToDocxConvertToXmlNotNull()
        {
            DocToXml converter = new DocToXml();
            string path = "./Files/doc1.doc";
            using var fs = File.OpenRead(path);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
