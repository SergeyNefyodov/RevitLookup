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
using LookupEngine.Abstractions.Enums;
using LookupEngine.Abstractions.Metadata;
using LookupEngine.Formaters;

namespace LookupEngine;

public sealed partial class LookupComposer
{
    private void WriteDescriptor(object? value, Type inputType)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            // Value = EvaluateValue(value),
            TypeFullName = ReflexionFormater.FormatTypeFullName(inputType),
            MemberAttributes = MemberAttributes.Property,
            Type = ReflexionFormater.FormatTypeName(inputType)
        };

        // descriptor.Name = descriptor.Value.Descriptor.Type;
        _descriptors.Add(descriptor);
    }

    private void WriteDescriptor(object? value, Type inputType, string name)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            Name = name,
            // Value = EvaluateValue(value),
            TypeFullName = ReflexionFormater.FormatTypeFullName(inputType),
            MemberAttributes = MemberAttributes.Extension,
            Type = ReflexionFormater.FormatTypeName(inputType),
            ComputationTime = _clockDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        _descriptors.Add(descriptor);
    }

    private void WriteDescriptor(object? value, Type inputType, MemberInfo member, ParameterInfo[]? parameters = null)
    {
        var descriptor = new ObjectDescriptor
        {
            Depth = _depth,
            TypeFullName = ReflexionFormater.FormatTypeFullName(inputType),
            // Value = EvaluateValue(member, value),
            Name = ReflexionFormater.FormatMemberName(member, parameters),
            MemberAttributes = ModifiersFormater.FormatAttributes(member),
            Type = ReflexionFormater.FormatTypeName(inputType),
            ComputationTime = _clockDiagnoser.GetElapsed().TotalMilliseconds,
            AllocatedBytes = _memoryDiagnoser.GetAllocatedBytes()
        };

        _descriptors.Add(descriptor);
    }

    // private SnoopableObject EvaluateValue(MemberInfo member, object? value)
    // {
    //     var snoopableObject = new SnoopableObject(value, Context);
    //     SnoopUtils.Redirect(member.Name, snoopableObject);
    //     return snoopableObject;
    // }
    //
    // private SnoopableObject EvaluateValue(object? value)
    // {
    //     var snoopableObject = new SnoopableObject(value, Context);
    //     SnoopUtils.Redirect(snoopableObject);
    //     return snoopableObject;
    // }
}