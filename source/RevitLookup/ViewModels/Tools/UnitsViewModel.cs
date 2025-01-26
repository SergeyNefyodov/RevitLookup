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

using System.Diagnostics.CodeAnalysis;
using RevitLookup.Abstractions.Models.Tools;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.ViewModels.Tools;
using RevitLookup.Core;
using RevitLookup.Core.Units;

namespace RevitLookup.ViewModels.Tools;

[UsedImplicitly]
public sealed partial class UnitsViewModel(IVisualDecompositionService decompositionService) : ObservableObject, IUnitsViewModel
{
    [ObservableProperty] private List<UnitInfo> _units = [];
    [ObservableProperty] private List<UnitInfo> _filteredUnits = [];
    [ObservableProperty] private string _searchText = string.Empty;

    public void InitializeParameters()
    {
        Units = UnitsCollector.GetBuiltinParametersInfo();
    }

    public void InitializeCategories()
    {
        Units = UnitsCollector.GetBuiltinCategoriesInfo();
    }

    public void InitializeForgeSchema()
    {
        Units = UnitsCollector.GetForgeInfo();
    }

    public async Task DecomposeAsync(UnitInfo unitInfo)
    {
        var obj = unitInfo.Value switch
        {
            BuiltInParameter parameter => RevitShell.GetBuiltinParameter(parameter),
            BuiltInCategory category => RevitShell.GetBuiltinCategory(category),
            _ => unitInfo.Value
        };

        await decompositionService.VisualizeDecompositionAsync(obj);
    }

    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
    async partial void OnSearchTextChanged(string value)
    {
        try
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                FilteredUnits = Units;
                return;
            }

            FilteredUnits = await Task.Run(() =>
            {
                var formattedText = value.Trim();
                var searchResults = new List<UnitInfo>();
                foreach (var family in Units)
                {
                    if (family.Label.Contains(formattedText, StringComparison.OrdinalIgnoreCase) ||
                        family.Unit.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                    {
                        searchResults.Add(family);
                    }
                }

                return searchResults;
            });
        }
        catch
        {
            // ignored
        }
    }

    partial void OnUnitsChanged(List<UnitInfo> value)
    {
        FilteredUnits = value;
    }
}