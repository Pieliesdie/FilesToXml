﻿using ConverterToXml.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace ConverterToXml.Test
{
    public class XlsToXlsxTest
    {
        [Fact]
        public void XlsxConverterTestNotNull()
        {
            XlsToXlsx converter = new XlsToXlsx();
            string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = curDir + @"/Files/xls.xls";
            converter.ConvertToXlsxFile(path, curDir + @"/Files/Result.xlsx");
            Assert.True(File.Exists(curDir + @"/Files/Result.xlsx"));
        }
    }
}
