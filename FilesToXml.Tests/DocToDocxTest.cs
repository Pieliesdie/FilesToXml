using System.IO;
using FilesToXml.Core.Converters.OfficeConverters;
using Xunit;

namespace FilesToXml.Tests;

[Collection("DocToXml")]
public class DocToDocxTest
{
    [Fact]
    public void DocToDocxConvertToDocxNotNull()
    {
        var converter = new DocToDocx();
        var path = "./Files/doc1.doc";
        converter.ConvertFromFileToDocxFile(path, "./Files/Result.docx");
        Assert.True(File.Exists("./Files/Result.docx"));
    }
}