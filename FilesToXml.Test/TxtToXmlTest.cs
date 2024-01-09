using System.IO;
using FilesToXml.Core.Converters;
using Xunit;

namespace FilesToXml.Test
{
    public class TxtToXmlTest
    {
        [Fact]
        public void TxtToXmlTestNotNull()
        {
            var converter = new TxtToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/txt.txt";

            string result = converter.ConvertByFile(path).ToString();
            Assert.NotNull(result);
        }

        [Fact]
        public void TxtConvertToXmlNotNull()
        {
            var converter = new TxtToXml();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"\Files\txt.txt";
            string result = converter.Convert(File.Open(path, FileMode.Open)).ToString();
            Assert.NotNull(result);
        }
    }

}
