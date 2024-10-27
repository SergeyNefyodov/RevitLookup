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

using JetBrains.Annotations;
using LookupEngine.Abstractions.Metadata;
using LookupEngine.Diagnostic;
using LookupEngine.Options;

namespace LookupEngine;

[PublicAPI]
public sealed partial class LookupComposer
{
    private readonly DecomposeOptions _options;
    private readonly List<Descriptor> _descriptors = new(32);

    private int _depth;
    private object? _obj;

    private readonly ClockDiagnoser _clockDiagnoser = new();
    private readonly MemoryDiagnoser _memoryDiagnoser = new();

    private LookupComposer(DecomposeOptions options)
    {
        _options = options;
    }

    [Pure]
    public static IList<Descriptor> Decompose(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return [];

        options ??= DecomposeOptions.Default;
        var composer = new LookupComposer(options);

        return value switch
        {
            Type staticObjectType => composer.DecomposeStaticObject(staticObjectType),
            _ => composer.DecomposeInstanceObject(value)
        };
    }
}

// private Descriptor FindSuitableDescriptor(object? obj)
// {
//     var descriptor = _options.TypeResolver.Invoke(obj, null);
//
//     if (obj is null)
//     {
//         descriptor.Type = nameof(Object);
//         descriptor.TypeFullName = "System.Object";
//     }
//     else
//     {
//         var type = obj.GetType();
//         FormatDefaultProperties(descriptor, type);
//     }
//
//     return descriptor;
// }
//
// private Descriptor FindSuitableDescriptor(Type? type)
// {
//     var descriptor = new ObjectDescriptor();
//
//     if (type is null)
//     {
//         descriptor.Type = nameof(Object);
//         descriptor.TypeFullName = "System.Object";
//     }
//     else
//     {
//         FormatDefaultProperties(descriptor, type);
//     }
//
//     return descriptor;
// }
//
// private Descriptor FindSuitableDescriptor(object obj, Type type)
// {
//     var descriptor = _options.TypeResolver.Invoke(obj, type);
//
//     FormatDefaultProperties(descriptor, type);
//     return descriptor;
// }