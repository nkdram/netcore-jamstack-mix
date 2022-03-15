using System;
using System.Collections.Generic;
using System.Text;

namespace Sugcon.Foundation.GrapheNetor.Rendering.Extension
{
    public static class LinqExtensions
    {
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource item in source)
            {
                if (seenKeys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
    }
}
