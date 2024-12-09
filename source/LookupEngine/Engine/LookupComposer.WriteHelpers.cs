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
            RawValue = null,
            TypeName = nameof(Object),
            TypeFullName = $"{nameof(System)}.{nameof(Object)}"
        };
    }

    private static DecomposedObject CreateInstanceDecomposition(object instance, Type type, Descriptor descriptor)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(type);

        return new DecomposedObject
        {
            Name = string.IsNullOrEmpty(descriptor.Name) ? formatTypeName : descriptor.Name!,
            RawValue = instance,
            TypeName = formatTypeName,
            TypeFullName = $"{type.Namespace}.{formatTypeName}",
            Descriptor = descriptor
        };
    }

    private static DecomposedObject CreateStaticDecomposition(Type type, Descriptor descriptor)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(type);

        return new DecomposedObject
        {
            Name = string.IsNullOrEmpty(descriptor.Name) ? formatTypeName : descriptor.Name!,
            RawValue = type,
            TypeName = formatTypeName,
            TypeFullName = $"{type.Namespace}.{formatTypeName}",
            Descriptor = descriptor
        };
    }

    private void WriteEnumerableMember(object? value, int index)
    {
        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = CreateValue(nameof(IEnumerable), value),
            Name = $"{DeclaringType.Name.Replace("[]", string.Empty)}[{index}]",
            MemberAttributes = MemberAttributes.Property,
            DeclaringTypeName = nameof(IEnumerable),
            DeclaringTypeFullName = $"{nameof(System)}.{nameof(System.Collections)}.{nameof(IEnumerable)}",
        };

        DecomposedMembers.Add(member);
    }

    private DecomposedValue CreateNullableValue()
    {
        return new DecomposedValue
        {
            RawValue = null,
            Name = string.Empty,
            TypeName = nameof(Object),
            TypeFullName = $"{nameof(System)}.{nameof(Object)}"
        };
    }

    private DecomposedValue CreateValue(string targetMember, object? value)
    {
        if (value is null) return CreateNullableValue();

        var valueDescriptor = RedirectValue(targetMember, ref value);
        var valueType = value.GetType();
        var formatTypeName = ReflexionFormater.FormatTypeName(valueType);

        return new DecomposedValue
        {
            RawValue = value,
            Name = string.IsNullOrEmpty(valueDescriptor.Name) ? formatTypeName : valueDescriptor.Name!,
            TypeName = formatTypeName,
            TypeFullName = $"{valueType.Namespace}.{formatTypeName}",
            Descriptor = valueDescriptor
        };
    }

    private void WriteExtensionMember(object? value, string name)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(DeclaringType);

        var member = new DecomposedMember
        {
            Depth = _depth,
            Name = name,
            Value = CreateValue(name, value),
            DeclaringTypeName = formatTypeName,
            DeclaringTypeFullName = $"{DeclaringType.Namespace}.{formatTypeName}",
            MemberAttributes = MemberAttributes.Extension,
            ComputationTime = _timeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedMembers.Add(member);
    }

    private void WriteDecompositionMember(object? value, MemberInfo memberInfo, ParameterInfo[]? parameters = null)
    {
        var formatTypeName = ReflexionFormater.FormatTypeName(DeclaringType);

        var member = new DecomposedMember
        {
            Depth = _depth,
            Value = CreateValue(memberInfo.Name, value),
            Name = ReflexionFormater.FormatMemberName(memberInfo, parameters),
            DeclaringTypeName = formatTypeName,
            DeclaringTypeFullName = $"{DeclaringType.Namespace}.{formatTypeName}",
            MemberAttributes = ModifiersFormater.FormatAttributes(memberInfo),
            ComputationTime = _timeDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        DecomposedMembers.Add(member);
    }
}