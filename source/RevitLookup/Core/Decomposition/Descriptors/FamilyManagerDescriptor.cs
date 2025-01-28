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
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Decomposition.Descriptors;

public sealed class FamilyManagerDescriptor(FamilyManager familyManager) : Descriptor, IDescriptorResolver, IDescriptorResolver<Document>
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyManager.IsParameterLockable) => ResolveIsParameterLockable,
            nameof(FamilyManager.IsParameterLocked) => ResolveIsParameterLocked,
            _ => null
        };

        IVariant ResolveIsParameterLockable()
        {
            var familyParameters = familyManager.Parameters;
            var variants = Variants.Values<bool>(familyParameters.Size);
            foreach (FamilyParameter parameter in familyParameters)
            {
                var result = familyManager.IsParameterLockable(parameter);
                variants.Add(result, $"{parameter.Definition.Name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveIsParameterLocked()
        {
            var familyParameters = familyManager.Parameters;
            var variants = Variants.Values<bool>(familyParameters.Size);
            foreach (FamilyParameter parameter in familyParameters)
            {
                var result = familyManager.IsParameterLocked(parameter);
                variants.Add(result, $"{parameter.Definition.Name}: {result}");
            }

            return variants.Consume();
        }
    }

    Func<Document, IVariant>? IDescriptorResolver<Document>.Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilyManager.GetAssociatedFamilyParameter) => ResolveGetAssociatedFamilyParameter,
            _ => null
        };

        IVariant ResolveGetAssociatedFamilyParameter(Document context)
        {
            var elementTypes = context.GetElements().WhereElementIsElementType();
            var elementInstances = context.GetElements().WhereElementIsNotElementType();
            var elements = elementTypes
                .UnionWith(elementInstances)
                .ToElements();

            var variants = Variants.Values<KeyValuePair<Parameter, FamilyParameter>>(elements.Count);
            foreach (var element in elements)
            {
                foreach (Parameter parameter in element.Parameters)
                {
                    var familyParameter = familyManager.GetAssociatedFamilyParameter(parameter);
                    if (familyParameter is not null)
                    {
                        variants.Add(new KeyValuePair<Parameter, FamilyParameter>(parameter, familyParameter));
                    }
                }
            }

            return variants.Consume();
        }
    }
}