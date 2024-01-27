using System.Text;
using System.Xml.Linq;
using FilesToXml.Core;
using CommandLine;
using FilesToXml.Core.Extensions;

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
        using var output = new MemoryStream();
        using var errors = new MemoryStream();
        using var logs = new MemoryStream();
        using var outSw = new StreamWriter(output);
        using var errSw = new StreamWriter(errors);
        using var logSw = new StreamWriter(logs);
        IEnumerable<FileInformation> files = new List<FileInformation>();
        foreach (var file in options.Input) { }

        _ = ConverterToXml.Convert(files, options, outSw, errSw, logSw);
        using var outSr = new StreamReader(output, Encoding.GetEncoding(options.OutputCodepage));
        using var errorSr = new StreamReader(errors, Encoding.GetEncoding(options.OutputCodepage));
        return new ConvertResult()
        {
            Result = outSr.ReadToEnd(),
            Error = errorSr.ReadToEnd()
        };
    }
}

public class ConvertResult
{
    public string? Result { get; set; }
    public string? Error { get; set; }
}

public class FileOption
{
    public byte[] Data { get; set; }
    public string Path { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Codepage { get; set; } = 65001;
    public string Delimiter { get; set; } = "auto";
    public char[] SearchingDelimiters { get; set; } = new[] {';', '|', '\t', ','};
    public FileInformation MapToFileInforamtion()
    {
        return new FileInformation()
        {
            Stream = new MemoryStream(Data),
            Delimiter = Delimiter,
            Encoding = Encoding.GetEncoding(Codepage),
            Label = Label,
            Name = System.IO.Path.GetFileName(Path),
            Path = Path,
            SearchingDelimiters = SearchingDelimiters,
            Type = Path.ToFiletype()
        };
    }
}

public class ConvertOptions : IResultOptions
{
    public IEnumerable<FileOption> Input { get; init; }
    public string? Output { get; set; }
    public int OutputCodepage { get; set; } = 65001;
    public bool ForceSave { get; set; }
    public bool DisableFormat { get; set; }
}