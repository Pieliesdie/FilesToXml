using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultOptions : IOptions
{
    public string? Output { get; init; }
    public bool ForceSave { get; set; } = false;
    public int CodePage { get; init; } = DefaultValue.Encoding.CodePage;
    public bool DisableFormat { get; init; } = false;
    public IEnumerable<IFile> Files { get; init; } = [];
}