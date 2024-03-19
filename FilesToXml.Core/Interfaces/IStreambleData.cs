using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FilesToXml.Core.Interfaces;

public interface IStreambleData : IDisposable
{
    bool TryGetStream(TextWriter err, [NotNullWhen(true)] out Stream? stream);
}