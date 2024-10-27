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
using LookupEngine.Abstractions.Metadata;

namespace LookupEngine;

public sealed partial class LookupComposer
{
    private IList<Descriptor> DecomposeInstanceObject(object instance)
    {
        _obj = instance;
        var objectType = _obj.GetType();
        var objectTypeHierarchy = GetTypeHierarchy(objectType);

        for (var i = objectTypeHierarchy.Count - 1; i >= 0; i--)
        {
            var type = objectTypeHierarchy[i];
            var descriptor = _options.TypeResolver.Invoke(_obj, type);

            var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (!_options.IgnoreStatic) flags |= BindingFlags.Static;
            if (!_options.IgnorePrivate) flags |= BindingFlags.NonPublic;

            DecomposeFields(type, flags);
            DecomposeProperties(type, descriptor, flags);
            DecomposeMethods(type, descriptor, flags);
            DecomposeEvents(type, descriptor, flags);
            // AddExtensions(descriptor);

            _depth--;
        }

        AddEnumerableItems();

        return _descriptors;
    }

    private IList<Descriptor> DecomposeStaticObject(Type objectType)
    {
        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
        if (!_options.IgnorePrivate) flags |= BindingFlags.NonPublic;

        DecomposeFields(objectType, flags);
        DecomposeProperties(objectType, null, flags);
        DecomposeMethods(objectType, null, flags);

        return _descriptors;
    }

    private List<Type> GetTypeHierarchy(Type inputType)
    {
        var types = new List<Type>();
        while (inputType.BaseType is not null)
        {
            types.Add(inputType);
            inputType = inputType.BaseType;
        }

        if (!_options.IgnoreRoot) types.Add(inputType);

        return types;
    }
}