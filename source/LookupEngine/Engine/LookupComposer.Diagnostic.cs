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
using LookupEngine.Abstractions.Collections;
using LookupEngine.Diagnostic;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    private readonly TimeDiagnoser _timeDiagnoser = new();
    private readonly MemoryDiagnoser _memoryDiagnoser = new();

    private object? EvaluateValue(FieldInfo member)
    {
        _timeDiagnoser.StartMonitoring();
        _memoryDiagnoser.StartMonitoring();

        var value = member.GetValue(InputObject);

        _memoryDiagnoser.StopMonitoring();
        _timeDiagnoser.StopMonitoring();

        return value;
    }

    private object? EvaluateValue(PropertyInfo member)
    {
        try
        {
            _timeDiagnoser.StartMonitoring();
            _memoryDiagnoser.StartMonitoring();

            return member.GetValue(InputObject);
        }
        finally
        {
            _memoryDiagnoser.StopMonitoring();
            _timeDiagnoser.StopMonitoring();
        }
    }

    private object? EvaluateValue(MethodInfo member)
    {
        try
        {
            _timeDiagnoser.StartMonitoring();
            _memoryDiagnoser.StartMonitoring();

            return member.Invoke(InputObject, null);
        }
        finally
        {
            _memoryDiagnoser.StopMonitoring();
            _timeDiagnoser.StopMonitoring();
        }
    }

    private IVariants EvaluateValue(Func<IVariants> handler)
    {
        try
        {
            _timeDiagnoser.StartMonitoring();
            _memoryDiagnoser.StartMonitoring();

            return handler.Invoke();
        }
        finally
        {
            _memoryDiagnoser.StopMonitoring();
            _timeDiagnoser.StopMonitoring();
        }
    }
}