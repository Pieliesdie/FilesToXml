using System.Text;
using FilesToXml.Core;
using FilesToXml.Core.Extensions;
using FilesToXml.WPF.Helpers;

namespace FilesToXml.WPF.Model;

public record FileUpload
{
    private readonly string filePath = string.Empty;

    public FileUpload()
    {
    }

    public string Path
    {
        get => filePath;
        init
        {
            filePath = value;
            Extension = Path.ToFiletype().ToString().ToLower();
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
