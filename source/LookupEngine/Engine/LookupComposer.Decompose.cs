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

using System.Reflection;
using LookupEngine.Abstractions;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    private DecomposedObject DecomposeInstanceObject(object instance)
    {
        var objectType = instance.GetType();
        var objectTypeHierarchy = GetTypeHierarchy(objectType);
        var instanceDescriptor = _options.TypeResolver.Invoke(instance, null);
        _decomposedObject = CreateInstanceDecomposition(instance, objectType, instanceDescriptor);

        for (var i = objectTypeHierarchy.Count - 1; i >= 0; i--)
        {
            Subtype = objectTypeHierarchy[i];
            SubtypeDescriptor = _options.TypeResolver.Invoke(instance, Subtype);

            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (!_options.IgnoreStaticMembers) flags |= BindingFlags.Static;
            if (!_options.IgnorePrivateMembers) flags |= BindingFlags.NonPublic;

            DecomposeFields(flags);
            DecomposeProperties(flags);
            DecomposeMethods(flags);
            DecomposeEvents(flags);
            ExecuteExtensions();

            _depth--;
        }

        Subtype = objectType;
        AddEnumerableItems();

        return _decomposedObject;
    }

    private DecomposedObject DecomposeStaticObject(Type objectType)
    {
        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (!_options.IgnorePrivateMembers) flags |= BindingFlags.NonPublic;

        var staticDescriptor = _options.TypeResolver.Invoke(null, objectType);
        _decomposedObject = CreateStaticDecomposition(objectType, staticDescriptor);

        Subtype = objectType;
        SubtypeDescriptor = staticDescriptor;

        DecomposeFields(flags);
        DecomposeProperties(flags);
        DecomposeMethods(flags);

        return _decomposedObject;
    }

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