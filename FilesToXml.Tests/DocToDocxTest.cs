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
        var path = "./Files/doc1.doc";
        DocToDocx.Convert(path, "./Files/Result.docx");
        Assert.True(File.Exists("./Files/Result.docx"));
    }
}