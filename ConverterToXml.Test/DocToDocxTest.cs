﻿using System.IO;
using Xunit;

namespace ConverterToXml.Test
{
    [Collection("DocToXml")]
    public class DocToDocxTest
    {
        [Fact]
        public void DocToDocxConvertToDocxNotNull()
        {
            Converters.DocToDocx converter = new Converters.DocToDocx();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/doc1.doc";
            converter.ConvertFromFileToDocxFile(path, curDir + @"/Files/Result.docx");
            Assert.True(File.Exists(curDir + @"/Files/Result.docx"));
        }

    }
}
