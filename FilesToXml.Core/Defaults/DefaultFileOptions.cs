using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultFileOptions : IFileOptions
{
    public required string Path { get; init; }
    public int CodePage { get; init; } = DefaultValue.Encoding.CodePage;
    public string? Label { get; init; }
    public string Delimiter { get; set; } = DefaultValue.Delimiter;
    public char[] SearchingDelimiters { get; set; } = DefaultValue.SearchingDelimiters;
}