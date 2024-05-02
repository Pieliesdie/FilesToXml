using System;
using System.Collections;
using System.Collections.Generic;
using FilesToXml.Core.Extensions;

namespace FilesToXml.Core.Helpers;

public class CachingFirstElementEnumerable<T> : IEnumerable<T?>
{
    private readonly IEnumerable<T> source;
    private T? cachedFirstElement;
    private bool cacheFilled;
    
    public CachingFirstElementEnumerable(IEnumerable<T> source)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
        cacheFilled = false;
    }
    
    public IEnumerator<T?> GetEnumerator()
    {
        if (!cacheFilled)
        {
            // Получаем первый элемент и кэшируем его
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    cachedFirstElement = enumerator.Current;
                }
            }
            
            cacheFilled = true;
        }
        
        // Возвращаем остальные элементы из исходной последовательности
        foreach (var item in source.WithIndex())
        {
            if (item.index == 0)
            {
                yield return cachedFirstElement;
                continue;
            }
            
            yield return item.item;
        }
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}