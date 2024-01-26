using System.Collections.Generic;

namespace FilesToXml.Core;

public interface IOptions : IFileOptions, IOutputOptions { }

public interface IFileOptions
{
    IEnumerable<string> Delimiters { get; }
    IEnumerable<string> Input { get; set; }
    IEnumerable<int> InputEncoding { get; }
    IEnumerable<string>? Labels { get; }
    IEnumerable<char> SearchingDelimiters { get; }
}
public interface IOutputOptions
{
    bool DisableFormat { get; }
    bool ForceSave { get; }
    string? Output { get; }
    int OutputEncoding { get; }
}