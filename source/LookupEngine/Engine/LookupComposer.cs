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
using LookupEngine.Abstractions;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Abstractions.Descriptors;
using LookupEngine.Exceptions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

[PublicAPI]
public sealed partial class LookupComposer
{
    private readonly object _input;
    private readonly DecomposeOptions _options;

    private int _depth;
    private Type? _declaringType;
    private Descriptor? _declaringDescriptor;
    private DecomposedObject? _decomposedObject;
    private List<DecomposedMember>? _decomposedMembers;

    private LookupComposer(object value, DecomposeOptions options)
    {
        _input = value;
        _options = options;
    }

    internal List<DecomposedMember> DecomposedMembers
    {
        get
        {
            if (_decomposedMembers is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(DecomposedMembers));
            }

            return _decomposedMembers;
        }
        set => _decomposedMembers = value;
    }

    internal Type DeclaringType
    {
        get
        {
            if (_declaringType is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(DeclaringType));
            }

            return _declaringType;
        }
        set => _declaringType = value;
    }

    internal Descriptor DeclaringDescriptor
    {
        get
        {
            if (_declaringDescriptor is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(DeclaringDescriptor));
            }

            return _declaringDescriptor;
        }
        set => _declaringDescriptor = value;
    }

    [Pure]
    public static DecomposedObject Decompose(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return CreateNullableDecomposition();

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options)
                .DecomposeType(type, true),

            IVariant variant => new LookupComposer(variant.Value, options)
                .DecomposeInstance(true)
                .WithDescription(variant.Description),

            _ => new LookupComposer(value, options)
                .DecomposeInstance(true)
        };
    }

    [Pure]
    public static DecomposedObject DecomposeObject(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return CreateNullableDecomposition();

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options)
                .DecomposeType(type, false),

            IVariant variant => new LookupComposer(variant.Value, options)
                .DecomposeInstance(false)
                .WithDescription(variant.Description),

            _ => new LookupComposer(value, options)
                .DecomposeInstance(false)
        };
    }

    [Pure]
    public static List<DecomposedMember> DecomposeMembers(object? value, DecomposeOptions? options = null)
    {
        if (value is null) return [];

        options ??= DecomposeOptions.Default;
        return value switch
        {
            Type type => new LookupComposer(value, options).DecomposeTypeMembers(type),
            _ => new LookupComposer(value, options).DecomposeInstanceMembers(value.GetType())
        };
    }
}