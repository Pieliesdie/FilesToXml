using System.Diagnostics.CodeAnalysis;

namespace FilesToXml.Core.Interfaces;

public interface IStreambleData : IDisposable
{
    bool TryGetStream(TextWriter err, [NotNullWhen(true)] out Stream? stream);
}