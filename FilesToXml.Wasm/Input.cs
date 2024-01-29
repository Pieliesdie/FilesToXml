using FilesToXml.Core.Interfaces;

namespace FilesToXml.Wasm;

public class Input : IResultOptions
{
    public bool DisableFormat { get; init; }
    public required File[] Files { get; init; }
}