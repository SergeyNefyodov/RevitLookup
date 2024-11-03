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
using LookupEngine.Abstractions.ComponentModel;
using LookupEngine.Exceptions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

[PublicAPI]
public sealed partial class LookupComposer
{
    private readonly DecomposeOptions _options;
    private readonly List<DecompositionMemberData> _descriptors = new(32);

    private int _depth;
    private object? _inputObject;
    private Type? _subtype;
    private Descriptor? _subtypeDescriptor;

    private LookupComposer(DecomposeOptions options)
    {
        _options = options;
    }

    [Pure]
    public static List<DecompositionMemberData> Decompose(object? value, DecomposeOptions? options = null)
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

    internal object InputObject
    {
        get
        {
            if (_inputObject is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(InputObject));
            }

            return _inputObject;
        }
        set => _inputObject = value;
    }

    internal Type Subtype
    {
        get
        {
            if (_subtype is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(Subtype));
            }

            return _subtype;
        }
        set => _subtype = value;
    }

    internal Descriptor SubtypeDescriptor
    {
        get
        {
            if (_subtypeDescriptor is null)
            {
                EngineException.ThrowIfEngineNotInitialized(nameof(SubtypeDescriptor));
            }

            return _subtypeDescriptor;
        }
        set => _subtypeDescriptor = value;
    }
}