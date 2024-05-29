using System.Collections;

namespace FilesToXml.Core.Helpers;

public sealed class DisposableList<T> : List<T>, IDisposable
    where T : IDisposable
{
    public DisposableList(IEnumerable<T> items)
        : base(items) { }
    
    public void Dispose()
    {
        foreach (var item in this)
        {
            try
            {
                item.Dispose();
            }
            catch
            {
                // swallow
            }
        }
    }
}

public static class DisposableListExtension
{
    public static DisposableList<T> ToDisposableList<T>(this IEnumerable<T> src) where T : IDisposable
    {
        return new DisposableList<T>(src);
    }
}