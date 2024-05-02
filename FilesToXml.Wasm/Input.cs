using FilesToXml.Core.Interfaces;

namespace FilesToXml.Wasm;

public class Input : IResultOptions
{
    public required File[] Files { get; init; }
    public bool DisableFormat { get; init; }
}