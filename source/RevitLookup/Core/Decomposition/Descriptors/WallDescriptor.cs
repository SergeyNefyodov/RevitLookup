// Copyright 2003-2025 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Electrical;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public class WallDescriptor(Wall wall) : ElementDescriptor(wall)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(Wall.IsWallCrossSectionValid) => ResolveIsWallCrossSectionValid,
            _ => null
        };

        IVariant ResolveIsWallCrossSectionValid()
        {
            var values = Enum.GetValues(typeof(WallCrossSection));
            var variants = Variants.Values<bool>(values.Length);

            foreach (WallCrossSection crossSection in values)
            {
                var result = wall.IsWallCrossSectionValid(crossSection);
                variants.Add(result, $"{crossSection}: {result}");
            }

            return variants.Consume();
        }
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}