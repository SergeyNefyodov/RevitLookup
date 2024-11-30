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
using LookupEngine.Abstractions.ComponentModel;
using LookupEngine.Abstractions.Enums;
using LookupEngine.Formaters;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    private static DecomposedObject CreateNullableDecomposition()
    {
        return new DecomposedObject
        {
            Name = $"{nameof(System)}.{nameof(Object)}",
            Value = null,
            Members = [],
            Type = nameof(Object),
            TypeFullName = $"{nameof(System)}.{nameof(Object)}"
        };
    }

    private static DecomposedObject CreateInstanceDecomposition(object instance, Type type, Descriptor descriptor)
    {
        return new DecomposedObject
        {
            Name = descriptor.Name ?? type.Name,
            Value = instance,
            Type = ReflexionFormater.FormatTypeName(type),
            TypeFullName = ReflexionFormater.FormatTypeFullName(type),
            Members = new List<DecomposedMember>(32)
        };
    }

    private static DecomposedObject CreateStaticDecomposition(Type type, Descriptor descriptor)
    {
        return new DecomposedObject
        {
            Name = descriptor.Name ?? type.Name,
            Value = type,
            Type = ReflexionFormater.FormatTypeName(type),
            TypeFullName = ReflexionFormater.FormatTypeFullName(type),
            Members = new List<DecomposedMember>(32)
        };
    }

    private void WriteEnumerableMember(object? value, int index)
    {
        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = value,
            // Value = RedirectValue(value),
            Name = $"{Subtype.Name}[{index}]",
            MemberAttributes = MemberAttributes.Property,
            Type = nameof(IEnumerable),
            TypeFullName = $"{nameof(System)}.{nameof(System.Collections)}.{nameof(IEnumerable)}",
        };

        DecomposedObject.Members.Add(member);
    }

    private void WriteExtensionMember(object? value, string name)
    {
        var member = new DecomposedMember
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

        DecomposedObject.Members.Add(member);
    }

    private void WriteDecompositionMember(object? value, MemberInfo memberInfo, ParameterInfo[]? parameters = null)
    {
        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = value,
            // Value = RedirectValue(member, value),
            Name = ReflexionFormater.FormatMemberName(memberInfo, parameters),
            Type = ReflexionFormater.FormatTypeName(Subtype),
            TypeFullName = ReflexionFormater.FormatTypeFullName(Subtype),
            MemberAttributes = ModifiersFormater.FormatAttributes(memberInfo),
            ComputationTime = _timeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedObject.Members.Add(member);
    }
}