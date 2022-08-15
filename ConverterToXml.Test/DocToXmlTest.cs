using System;
using System.IO;
using Xunit;

namespace ConverterToXml.Test
{
    [Collection("DocToXml")]
    public class DocToXmlTest
    {
        [Fact]
        public void DocToDocxConvertByFileToXmlNotNull()
        {
            Converters.DocToXml converter = new Converters.DocToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\doc1.doc";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void DocToDocxConvertToXmlNotNull()
        {
            Converters.DocToXml converter = new Converters.DocToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\doc1.doc";
            using var fs = File.Open(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
