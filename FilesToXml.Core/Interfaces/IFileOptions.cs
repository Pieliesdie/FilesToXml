using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesToXml.Core.Interfaces;

public interface IFileOptions : IDisposable
{
    string Path { get; }
    int CodePage { get; }
    string? Label { get; }
    string? Delimiter { get; }
    char[]? SearchingDelimiters { get; }
    bool TryGetData(TextWriter err, [NotNullWhen(true)] out Stream? stream);
}