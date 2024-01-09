using System.Text;
using FilesToXml.Core;
using FilesToXml.Winforms.Helpers;

namespace FilesToXml.Winforms.Model;

public record FileUpload
{
    private string filePath = string.Empty;

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
    public string Name { get; init; } = string.Empty;
    public long Size { get; init; } = 0;
    public string SizeInKb => $"{Size / 1024} kb";
    public string Label { get; set; } = string.Empty;
    public string Extension { get; private set; } = string.Empty;
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    public string Delimiter { get; set; } = "auto";
}
