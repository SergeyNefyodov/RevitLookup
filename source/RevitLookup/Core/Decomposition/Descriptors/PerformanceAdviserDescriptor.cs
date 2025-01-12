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

public sealed class PerformanceAdviserDescriptor(PerformanceAdviser adviser) : Descriptor, IDescriptorResolver, IDescriptorResolver<Document>
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(PerformanceAdviser.GetRuleDescription) when parameters!.Length == 1 &&
                                                               parameters[0].ParameterType == typeof(int) => ResolveGetRuleDescription,
            nameof(PerformanceAdviser.GetRuleId) when parameters!.Length == 1 &&
                                                      parameters[0].ParameterType == typeof(int) => ResolveGetRuleId,
            nameof(PerformanceAdviser.GetRuleName) when parameters!.Length == 1 &&
                                                        parameters[0].ParameterType == typeof(int) => ResolveGetRuleName,
            nameof(PerformanceAdviser.IsRuleEnabled) when parameters!.Length == 1 &&
                                                          parameters[0].ParameterType == typeof(int) => ResolveIsRuleEnabled,
            nameof(PerformanceAdviser.WillRuleCheckElements) when parameters!.Length == 1 &&
                                                                  parameters[0].ParameterType == typeof(int) => ResolveWillRuleCheckElements,
            nameof(PerformanceAdviser.GetElementFilterFromRule) when parameters!.Length == 2 &&
                                                                     parameters[0].ParameterType == typeof(int) => ResolveGetElementFilterFromRule,
            _ => null
        };

        IVariant ResolveGetElementFilterFromRule()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, ElementFilter>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, ElementFilter>(i, adviser.GetElementFilterFromRule(i, Context.ActiveDocument)));
            return variants.Consume();
        }

        IVariant ResolveWillRuleCheckElements()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, bool>>(rules);
            for (var i = 0; i < rules; i++)
            {
                variants.Add(new KeyValuePair<int, bool>(i, adviser.WillRuleCheckElements(i)));
            }

            return variants.Consume();
        }

        IVariant ResolveIsRuleEnabled()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, bool>>(rules);
            for (var i = 0; i < rules; i++)
            {
                variants.Add(new KeyValuePair<int, bool>(i, adviser.IsRuleEnabled(i)));
            }

            return variants.Consume();
        }

        IVariant ResolveGetRuleName()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, string>>(rules);
            for (var i = 0; i < rules; i++) variants.Add(new KeyValuePair<int, string>(i, adviser.GetRuleName(i)));
            return variants.Consume();
        }

        IVariant ResolveGetRuleId()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, PerformanceAdviserRuleId>>(rules);
            for (var i = 0; i < rules; i++)
            {
                variants.Add(new KeyValuePair<int, PerformanceAdviserRuleId>(i, adviser.GetRuleId(i)));
            }

            return variants.Consume();
        }

        IVariant ResolveGetRuleDescription()
        {
            var rules = adviser.GetNumberOfRules();
            var variants = Variants.Values<KeyValuePair<int, string>>(rules);
            for (var i = 0; i < rules; i++)
            {
                variants.Add(new KeyValuePair<int, string>(i, adviser.GetRuleDescription(i)));
            }

            return variants.Consume();
        }
    }

    Func<Document, IVariant>? IDescriptorResolver<Document>.Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(PerformanceAdviser.ExecuteAllRules) when parameters!.Length == 1 &&
                                                            parameters[0].ParameterType == typeof(Document) => ResolveExecuteAllRules,
            _ => null
        };

        IVariant ResolveExecuteAllRules(Document context)
        {
            return Variants.Value(adviser.ExecuteAllRules(context));
        }
    }
}