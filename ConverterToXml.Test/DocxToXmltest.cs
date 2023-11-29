using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConverterToXml.Core.Converters;
using Xunit;

namespace ConverterToXml.Test
{
    public class DocxToXmlTest
    {
        [Fact]
        public void DocxConverterTestNotNull()
        {
            DocxToXml converter = new DocxToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/docx.docx";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void DocxConvertToXmlNotNull()
        {
            var converter = new DocxToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\docx.docx";
            using var fs = File.Open(path, FileMode.Open);
            string result = converter.Convert(fs).ToString();
            Assert.NotNull(result);
        }
    }
}
