using System.Text;
using System.Xml.Linq;
using FilesToXml.Core;
using CommandLine;
namespace FilesToXml.Wasm;

public interface IConverter
{
    string Beautify(string xml);
    string GetBackendName();
    ConvertResult Convert(ConvertOptions options);

    void ShowFile(Stream fileStream);
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
        _ = ConverterToXml.Convert(options.MapToOptions(), output, errors);
        using var outSr = new StreamReader(output, Encoding.GetEncoding(options.OutputCodepage));
        using var errorSr = new StreamReader(errors, Encoding.GetEncoding(options.OutputCodepage));
        return new ConvertResult()
        {
            Result = outSr.ReadToEnd(),
            Error = errorSr.ReadToEnd()
        };
    }
    public void ShowFile(Stream fileStream)
    {
        using var sr = new StreamReader(fileStream);
        Console.WriteLine(sr.ReadToEnd());
    }
}

public class ConvertResult
{
    public string? Result { get; set; }
    public string? Error { get; set; }
}

public class FileOption
{
    public string Path { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Codepage { get; set; } = 65001;
    public string Delimiter { get; set; } = "auto";
}

public class ConvertOptions
{
    public IEnumerable<FileOption> Input { get; init; }
    public string? Output { get; set; }
    public int OutputCodepage { get; set; } = 65001;
    public bool ForceSave { get; set; }
    public bool DisableFormat { get; set; }
    public char[] SearchingDelimiters { get; set; }
    public IOptions MapToOptions()
    {
        return new Options()
        {
            Input = Input.Select(x => x.Path),
            DisableFormat = DisableFormat,
            ForceSave = ForceSave,
            InputEncoding = Input.Select(x => x.Codepage),
            Output = string.IsNullOrWhiteSpace(Output) ? null : Output,
            OutputEncoding = OutputCodepage,
            Delimiters = Input.Select(x => x.Delimiter),
            Labels = Input.Select(x => x.Label),
            SearchingDelimiters = SearchingDelimiters
        };
    }

    private class Options : DefaultOptions { }
}