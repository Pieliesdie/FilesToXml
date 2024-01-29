namespace FilesToXml.Core.Interfaces;

public interface IFileOptions
{
    string Path { get; }
    int CodePage { get; }
    string? Label { get; }
    string Delimiter { get; }
    char[] SearchingDelimiters { get; }
}