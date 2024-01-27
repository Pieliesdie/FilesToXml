namespace FilesToXml.Core.Interfaces;

public interface IOutputOptions
{
    string? Output { get; }
    bool ForceSave { get; }
    int OutputEncoding { get; }
}