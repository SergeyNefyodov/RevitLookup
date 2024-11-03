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
using System.Reflection;
using LookupEngine.Abstractions;
using LookupEngine.Abstractions.Enums;
using LookupEngine.Formaters;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    private void WriteEnumerableResult(object? value, int index)
    {
        var descriptor = new DecompositionMemberData
        {
            Depth = _depth,
            Value = value,
            // Value = RedirectValue(value),
            Name = $"{Subtype.Name}[{index}]",
            TypeFullName = "System.Runtime.IEnumerable",
            MemberAttributes = MemberAttributes.Property,
            Type = nameof(IEnumerable)
        };

        _descriptors.Add(descriptor);
    }

    private void WriteExtensionResult(object? value, string name)
    {
        var descriptor = new DecompositionMemberData
        {
            Depth = _depth,
            Name = name,
            Value = value,
            // Value = RedirectValue(value),
            Type = ReflexionFormater.FormatTypeName(Subtype),
            TypeFullName = ReflexionFormater.FormatTypeFullName(Subtype),
            MemberAttributes = MemberAttributes.Extension,
            ComputationTime = _timeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        _descriptors.Add(descriptor);
    }

    private void WriteDecompositionResult(object? value, MemberInfo member, ParameterInfo[]? parameters = null)
    {
        var descriptor = new DecompositionMemberData
        {
            Depth = _depth,
            Value = value,
            // Value = RedirectValue(member, value),
            Name = ReflexionFormater.FormatMemberName(member, parameters),
            Type = ReflexionFormater.FormatTypeName(Subtype),
            TypeFullName = ReflexionFormater.FormatTypeFullName(Subtype),
            MemberAttributes = ModifiersFormater.FormatAttributes(member),
            ComputationTime = _timeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        _descriptors.Add(descriptor);
    }
}