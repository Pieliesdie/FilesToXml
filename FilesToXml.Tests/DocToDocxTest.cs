using System.IO;
using FilesToXml.Core.Converters.OfficeConverters;
using Xunit;

namespace FilesToXml.Test
{
    [Collection("DocToXml")]
    public class DocToDocxTest
    {
        [Fact]
        public void DocToDocxConvertToDocxNotNull()
        {
            DocToDocx converter = new DocToDocx();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/doc1.doc";
            converter.ConvertFromFileToDocxFile(path, curDir + @"/Files/Result.docx");
            Assert.True(File.Exists(curDir + @"/Files/Result.docx"));
        }

    }
}
