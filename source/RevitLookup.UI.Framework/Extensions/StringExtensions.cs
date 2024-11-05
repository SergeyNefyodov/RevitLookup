using System;
using JetBrains.Annotations;

namespace RevitLookup.UI.Framework.Extensions;

public static class StringExtensions
{
#if !NETCORE
    /// <summary>
    ///     Returns a value indicating whether a specified substring occurs within this string.
    /// </summary>
    /// <param name="source">Source string</param>
    /// <param name="value">The string to seek</param>
    /// <param name="comparison">One of the enumeration values that specifies the rules for the search</param>
    /// <returns>True if the value parameter occurs within this string, or if value is the empty string (""); otherwise, false</returns>
    [Pure]
    [ContractAnnotation("source:null => false; value:null => false")]
    public static bool Contains(this string? source, string? value, StringComparison comparison)
    {
        if (source is null) return false;
        if (value is null) return false;
        return source.IndexOf(value, comparison) >= 0;
    }
#endif
}