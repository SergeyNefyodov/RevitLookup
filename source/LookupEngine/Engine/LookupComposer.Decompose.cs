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

using System.Diagnostics.Contracts;
using System.Reflection;
using LookupEngine.Abstractions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    [Pure]
    private DecomposedObject DecomposeInstance(bool decomposeMembers)
    {
        var objectType = _input.GetType();
        var instanceDescriptor = _options.TypeResolver.Invoke(_input, null);
        _decomposedObject = CreateInstanceDecomposition(_input, objectType, instanceDescriptor);

        if (decomposeMembers)
        {
            var members = DecomposeInstanceMembers(objectType);
            _decomposedObject.Members.AddRange(members);
        }

        return _decomposedObject;
    }

    [Pure]
    private DecomposedObject DecomposeType(Type type, bool decomposeMembers)
    {
        var staticDescriptor = _options.TypeResolver.Invoke(null, type);
        _decomposedObject = CreateStaticDecomposition(type, staticDescriptor);

        if (decomposeMembers)
        {
            var members = DecomposeTypeMembers(type);
            _decomposedObject.Members.AddRange(members);
        }

        return _decomposedObject;
    }

    [Pure]
    private List<DecomposedMember> DecomposeInstanceMembers(Type objectType)
    {
        _decomposedMembers = new List<DecomposedMember>(32);

        var objectTypeHierarchy = GetTypeHierarchy(objectType);
        for (var i = objectTypeHierarchy.Count - 1; i >= 0; i--)
        {
            DeclaringType = objectTypeHierarchy[i];
            DeclaringDescriptor = _options.TypeResolver.Invoke(_input, DeclaringType);

            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (_options.IncludeStaticMembers) flags |= BindingFlags.Static;
            if (_options.IncludePrivateMembers) flags |= BindingFlags.NonPublic;

            DecomposeFields(flags);
            DecomposeProperties(flags);
            DecomposeMethods(flags);
            DecomposeEvents(flags);
            ExecuteExtensions();

            _depth--;
        }

        DeclaringType = objectType;
        AddEnumerableItems();

        return _decomposedMembers;
    }

    [Pure]
    private List<DecomposedMember> DecomposeTypeMembers(Type type)
    {
        _decomposedMembers = new List<DecomposedMember>(32);

        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (_options.IncludePrivateMembers) flags |= BindingFlags.NonPublic;

        var objectTypeHierarchy = GetTypeHierarchy(type);
        for (var i = objectTypeHierarchy.Count - 1; i >= 0; i--)
        {
            DeclaringType = objectTypeHierarchy[i];
            DeclaringDescriptor = _options.TypeResolver.Invoke(null, DeclaringType);

            DecomposeFields(flags);
            DecomposeProperties(flags);
            DecomposeMethods(flags);

            _depth--;
        }

        return _decomposedMembers;
    }

    [Pure]
    private List<Type> GetTypeHierarchy(Type inputType)
    {
        var types = new List<Type>();
        while (inputType.BaseType is not null)
        {
            types.Add(inputType);
            inputType = inputType.BaseType;
        }

        if (_options.IncludeRoot) types.Add(inputType);

        return types;
    }
}