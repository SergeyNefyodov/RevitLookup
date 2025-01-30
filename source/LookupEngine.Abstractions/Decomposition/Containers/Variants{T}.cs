// Copyright 2003-2024 by Autodesk, Inc.
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
// 
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System.Collections;

namespace LookupEngine.Abstractions.Decomposition.Containers;

/// <summary>
///     Represents a collection of variants
/// </summary>
/// <typeparam name="T">The type of the variants</typeparam>
/// <param name="capacity">The initial variants capacity. Required for atomic performance optimizations</param>
internal sealed class Variants<T>(int capacity) : IVariant, IVariantsCollection<T>, IReadOnlyCollection<Variant>
{
    private readonly List<Variant> _items = new(capacity);

    /// <summary>
    ///     Gets the number of variants
    /// </summary>
    public int Count => _items.Count;

    public object Value => _items.Count == 1 ? _items[0].Value! : this;
    public string? Description => _items.Count == 1 ? _items[0].Description : null;

    /// <summary>
    ///     Adds a new variant
    /// </summary>
    /// <returns>The variant collection with a new value</returns>
    public IVariantsCollection<T> Add(T? result)
    {
        if (result is null) return this;
        if (result is ICollection {Count: 0}) return this;

        _items.Add(new Variant(result));

        return this;
    }

    /// <summary>
    ///     Adds a new variant with description
    /// </summary>
    /// <returns>The variant collection with a new value</returns>
    public IVariantsCollection<T> Add(T? result, string description)
    {
        if (result is null) return this;
        if (result is ICollection {Count: 0}) return this;

        _items.Add(new Variant(result, description));

        return this;
    }

    public IVariant Consume()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator<Variant> IEnumerable<Variant>.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}