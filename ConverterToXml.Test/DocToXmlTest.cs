using System;
using System.IO;
using Xunit;

namespace ConverterToXml.Test
{
    
    public class DocToXmlTest
    {
        [Fact]
        public void DocToDocxConvertToXmlNotNull()
        {
            Converters.DocToXml converter = new Converters.DocToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\doc1.doc";

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                string result = converter.ConvertByFile(path).ToString();
                Assert.NotNull(result);
            }
        }
    }
}
