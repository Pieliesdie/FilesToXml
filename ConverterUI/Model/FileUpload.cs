using ConverterToXml;

using System.Text;

namespace ConverterUI;

public record FileUpload
{
    private string filePath;

    public FileUpload()
    {
    }

    public string Path
    {
        get => filePath;
        init
        {
            filePath = value;
            Extension = Path.GetExtFromPath()?.ToString() ?? "Unsupported";
            Encoding = EncodingTools.GetEncoding(filePath);
        }
    }
    public string Name { get; init; }
    public long Size { get; init; }
    public string SizeInKb => $"{Size / 1024} kb";
    public string Label { get; set; }
    public string Extension { get; private set; }
    public Encoding Encoding { get; set; }
    public string Delimiter { get; set; } = "auto";
}
