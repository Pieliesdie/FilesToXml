﻿using ConverterToXml.Converters;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ConverterToXml.Test;
public class TsvToXmlTest
{
    [Fact]
    public void TsvToXmlByFileTestNotNull()
    {
        var converter = new TsvToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/tsv.tsv";

        string result = converter.ConvertByFile(path).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TsvConvertToXmlNotNull()
    {
        var converter = new TsvToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"\Files\tsv.tsv";
        using var fs = File.Open(path, FileMode.Open);
        string result = converter.Convert(fs).ToString();
        Assert.NotNull(result);
    }

    [Fact]
    public void TsvToXmlTestReadFirstLine()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var converter = new TsvToXml();
        string curDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string path = curDir + @"/Files/tsv.tsv";

        var result = converter.ConvertByFile(path);
        Assert.Equal("rice", result.Elements().First().Elements().First().Attribute("C6").Value);
    }

}