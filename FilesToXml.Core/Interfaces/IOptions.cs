using System.Collections.Generic;

namespace FilesToXml.Core.Interfaces;

public interface IOptions : IOutputOptions, IResultOptions
{
    IEnumerable<IFileOptions> FileOptions { get; }
}