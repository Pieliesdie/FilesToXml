using System.Collections.Generic;
using FilesToXml.Core.Interfaces;

namespace FilesToXml.Core.Defaults;

public class DefaultOptions : IOptions
{
    public string? Output { get; init; }
    public bool ForceSave { get; set; } = false;
    public required int OutputEncoding { get; init; }
    public bool DisableFormat { get; init; } = false;
    public required IEnumerable<IFileOptions> FileOptions { get; init; }
}