using System;
using System.Collections.Generic;
using System.Linq;

namespace FilesToXml.Core.Extensions;

public static class EnumerableExtensions
{
    public static T[] ToArrayOrEmpty<T>(this IEnumerable<T>? src)
    {
        return src?.ToArray() ?? Enumerable.Empty<T>().ToArray();
    }
    public static T ElementAtOrLast<T>(this List<T> source, int index)
    {
        return index > source.Count - 1 ? source.Last() : source.ElementAt(index);
    }
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
            action(element);
    }
}