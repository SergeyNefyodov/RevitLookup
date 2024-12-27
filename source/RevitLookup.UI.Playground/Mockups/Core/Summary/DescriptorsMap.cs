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
using System.Numerics;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Descriptors;
using RevitLookup.UI.Playground.Mockups.Core.Summary.Descriptors;

namespace RevitLookup.UI.Playground.Mockups.Core.Summary;

public static class DescriptorsMap
{
    /// <summary>
    ///     Search for a descriptor by approximate or exact match
    /// </summary>
    /// <remarks>
    ///     Exact search is necessary for the reflection engine, to add extensions and resolve conflicts when calling methods and properties. Type is not null <p />
    ///     An approximate search is needed to describe the object, which is displayed to the user. Type is null
    /// </remarks>
    public static Descriptor FindDescriptor(object? obj, Type? type)
    {
        return obj switch
        {
            bool value when type is null || type == typeof(bool) => new BooleanDescriptor(value),
            string value when type is null || type == typeof(string) => new StringDescriptor(value),
            IEnumerable value => new EnumerableDescriptor(value),
            Vector3 value => new Vector3Descriptor(value),
            Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
    }
}