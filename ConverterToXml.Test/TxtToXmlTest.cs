using ConverterToXml.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConverterToXml.Test
{
    public class TxtToXmlTest
    {
        [Fact]
        public void TxtToXmlTestNotNull()
        {
            var converter = new TxtToXml();
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/txt.txt";

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                string result = converter.Convert(fs).ToString();
                Assert.NotNull(result);
            }
        }
    }

}
