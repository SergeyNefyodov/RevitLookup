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

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class SunAndShadowSettingsDescriptor(SunAndShadowSettings settings) : ElementDescriptor(settings)
{
    public override Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(SunAndShadowSettings.GetActiveSunAndShadowSettings) => ResolveGet,
            nameof(SunAndShadowSettings.GetSunrise) => ResolveGetSunrise,
            nameof(SunAndShadowSettings.GetSunset) => ResolveGetSunset,
            nameof(SunAndShadowSettings.IsTimeIntervalValid) => ResolveTimeInterval,
            nameof(SunAndShadowSettings.IsAfterStartDateAndTime) => ResolveAfterStart,
            nameof(SunAndShadowSettings.IsBeforeEndDateAndTime) => ResolveBeforeStart,
            _ => null
        };

        IVariant ResolveGet()
        {
            return Variants.Value(SunAndShadowSettings.GetActiveSunAndShadowSettings(settings.Document));
        }

        IVariant ResolveGetSunrise()
        {
            return Variants.Value(settings.GetSunrise(DateTime.Today));
        }

        IVariant ResolveGetSunset()
        {
            return Variants.Value(settings.GetSunset(DateTime.Today));
        }

        IVariant ResolveAfterStart()
        {
            return Variants.Value(settings.IsAfterStartDateAndTime(DateTime.Today));
        }

        IVariant ResolveBeforeStart()
        {
            return Variants.Value(settings.IsBeforeEndDateAndTime(DateTime.Today));
        }

        IVariant ResolveTimeInterval()
        {
            var conditions = Enum.GetValues(typeof(SunStudyTimeInterval));
            var variants = Variants.Values<bool>(conditions.Length);

            foreach (SunStudyTimeInterval condition in conditions)
            {
                var result = settings.IsTimeIntervalValid(condition);
                variants.Add(result, $"{condition}: {result}");
            }

            return variants.Consume();
        }
    }

    public override void RegisterExtensions(IExtensionManager manager)
    {
    }
}