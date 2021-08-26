using System;
using System.Collections.Generic;
using System.Linq;
using Evolutex.Evolunity.Extensions;

namespace AppName.Core.Extensions
{
    /// <summary>
    ///     Extensions class just for example.
    ///     <seealso cref="Evolutex.Evolunity.Extensions.EnumerableExtensions" />
    /// </summary>
    public static class EnumerableExtensions
    {
        public static string AsString<T>(this IEnumerable<T> source)
        {
            return AsString(source, x => x?.ToString());
        }

        public static string AsString<T>(this IEnumerable<T> source, string separator)
        {
            return AsString(source, x => x?.ToString(), separator);
        }

        public static string AsString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator = ", ")
        {
            return source.IsEmpty()
                ? string.Empty
                : string.Join(separator, source.Select(x => selector(x) ?? "null"));
        }
    }
}