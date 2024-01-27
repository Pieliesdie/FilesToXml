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
    ConvertResult Convert(ConvertOptions options);
}

public class Converter : IConverter
{
    public string GetBackendName() => $".NET {Environment.Version}";
    public string Beautify(string xml) => XDocument.Parse(xml).ToString();
    public ConvertResult Convert(ConvertOptions options)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var outSw = new StreamWriter(new MemoryStream());
        var errSw = new StreamWriter(new MemoryStream());
        var logSw = new StreamWriter(new MemoryStream());

        _ = ConverterToXml.Convert(options.Input, options, outSw, errSw, logSw);
        
        var outSr = new StreamReader(outSw.BaseStream, Encoding.GetEncoding(options.OutputCodepage));
        var errorSr = new StreamReader(errSw.BaseStream, Encoding.GetEncoding(options.OutputCodepage));
        var logSr = new StreamReader(logSw.BaseStream, Encoding.GetEncoding(options.OutputCodepage));
        return new ConvertResult(outSr.ReadToEnd(), errorSr.ReadToEnd(), logSr.ReadToEnd());
    }
}

public record ConvertResult(string? Result, string? Error, string? Log);

public class FileOption : DefaultFileOptions
{
    public required string Data { get; init; }
    protected override bool TryOpenStream(string path, TextWriter err, out Stream? stream)
    {
        stream = new MemoryStream(Convert.FromBase64String(Data));
        return true;
    }
}

public class ConvertOptions : IResultOptions
{
    public required FileOption[] Input { get; init; }
    public string? Output { get; init; }
    public int OutputCodepage { get; init; } = 65001;
    public bool ForceSave { get; init; }
    public bool DisableFormat { get; init; }
}