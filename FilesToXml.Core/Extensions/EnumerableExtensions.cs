﻿using FilesToXml.Core.Helpers;

namespace FilesToXml.Core.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class
    {
        foreach (var t in enumerable)
        {
            if (t != null)
            {
                yield return t;
            }
        }
    }
    
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
        {
            action(element);
        }
    }
    
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
    {
        return self.Select((item, index) => (item, index));
    }
    
    public static IEnumerable<T?> CacheFirstElement<T>(this IEnumerable<T?> self)
    {
        return new CachingFirstElementEnumerable<T?>(self);
    }
}