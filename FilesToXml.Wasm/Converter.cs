using System.Text;
using System.Xml.Linq;
using FilesToXml.Core;
using FilesToXml.Core.Defaults;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Wasm;

public interface IConverter
{
    string Beautify(string xml);
    string GetBackendName();
    ConvertResult Convert(test data);
    long Size(byte[] self);
}

public class Converter : IConverter
{
    public string GetBackendName() => $".NET {Environment.Version}";
    public string Beautify(string xml) => XDocument.Parse(xml).ToString();
    public ConvertResult Convert(test data)
    {
        Console.WriteLine(data.i == null);
        Console.WriteLine(data.i?.Length);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var outSw = new StreamWriter(new MemoryStream(), leaveOpen: true);
        var errSw = new StreamWriter(new MemoryStream(), leaveOpen: true);
        var logSw = new StreamWriter(new MemoryStream(), leaveOpen: true);

        // var files = data.Select(x => new File()
        // {
        //     
        // });
        //_ = ConverterToXml.Convert(data, new DefaultOptions(), outSw, errSw, logSw);

        using var outSr = new StreamReader(outSw.BaseStream);
        using var errorSr = new StreamReader(errSw.BaseStream);
        using var logSr = new StreamReader(logSw.BaseStream);
        return new ConvertResult(outSr.ReadToEnd(), errorSr.ReadToEnd(), logSr.ReadToEnd());
    }
    public long Size(byte[] self)
    {
        return (new MemoryStream(self)).Length;
    }
}

public record test(byte[] i, int k);
public record ConvertResult(string? Result, string? Error, string? Log);

public class File
{
    public byte[] Data { get; set; }
    public string Path { get; set; }
    public int CodePage { get;  set; }
    public string? Label { get;  set; }
    public string Delimiter { get; set;  }
    public char[] SearchingDelimiters { get;  set; }
}