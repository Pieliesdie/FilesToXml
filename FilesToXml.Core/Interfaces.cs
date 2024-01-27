using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesToXml.Core;

public interface IOptions : IOutputOptions, IResultOptions
{
    IEnumerable<IFileOptions> FileOptions { get; }
}

public interface IOutputOptions
{
    string? Output { get; }
    bool ForceSave { get; }
    int OutputEncoding { get; }
}

public interface IResultOptions
{
    bool DisableFormat { get; }
}

public interface IFileOptions
{
    string Path { get; }
    bool TryGetData(TextWriter err, [NotNullWhen(true)] out Stream? stream);
    int InputEncoding { get; }
    string? Label { get; }
    string? Delimiter { get; }
    IEnumerable<char>? SearchingDelimiters { get; }
}