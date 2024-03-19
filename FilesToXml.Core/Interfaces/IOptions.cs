using System.Collections.Generic;

namespace FilesToXml.Core.Interfaces;

public interface IOptions : IOutputOptions, IResultOptions
{
    IEnumerable<IFile> Files { get; }
}