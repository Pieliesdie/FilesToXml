using System.Text;
using System.Xml.Linq;
using FilesToXml.Core;

namespace FilesToXml.Wasm;

public class Converter : IConverter
{
    public string GetBackendName()
    {
        return $".NET {Environment.Version}";
    }
    
    public string Beautify(string xml)
    {
        return XDocument.Parse(xml).ToString();
    }
    
    public ConvertResult Convert(Input data)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var outMs = new MemoryStream();
        using var errMs = new MemoryStream();
        using var logMs = new MemoryStream();
        using var outSw = new StreamWriter(outMs, leaveOpen: true) { AutoFlush = true };
        using var errSw = new StreamWriter(errMs, leaveOpen: true) { AutoFlush = true };
        using var logSw = new StreamWriter(logMs, leaveOpen: true) { AutoFlush = true };
        
        _ = ConverterToXml.Convert(data.Files, data, outSw, errSw, logSw);
        
        using var outSr = new StreamReader(outMs);
        using var errorSr = new StreamReader(errMs);
        using var logSr = new StreamReader(logMs);
        return new ConvertResult(outSr.ReadToEnd(), errorSr.ReadToEnd(), logSr.ReadToEnd());
    }
}

public record ConvertResult(string? Result, string? Error, string? Log);