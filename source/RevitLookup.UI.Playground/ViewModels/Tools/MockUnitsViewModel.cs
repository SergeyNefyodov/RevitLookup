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

using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Models.Tools;
using RevitLookup.Abstractions.ViewModels.Tools;
#if NETFRAMEWORK
using RevitLookup.UI.Framework.Extensions;
#endif

namespace RevitLookup.UI.Playground.ViewModels.Tools;

[UsedImplicitly]
public sealed partial class MockUnitsViewModel : ObservableObject, IUnitsViewModel
{
    [ObservableProperty] private List<UnitInfo> _units = [];
    [ObservableProperty] private List<UnitInfo> _filteredUnits = [];
    [ObservableProperty] private string _searchText = string.Empty;

    public void InitializeParameters()
    {
        Units = new Faker<UnitInfo>()
            .RuleFor(info => info.Unit, faker => faker.Lorem.Sentence())
            .RuleFor(info => info.Label, faker => faker.Lorem.Word())
            .RuleFor(info => info.Value, faker => faker.Lorem.Word())
            .Generate(200);
    }

    public void InitializeCategories()
    {
        Units = new Faker<UnitInfo>()
            .RuleFor(info => info.Unit, faker => faker.Lorem.Sentence())
            .RuleFor(info => info.Label, faker => faker.Lorem.Word())
            .RuleFor(info => info.Value, faker => faker.Lorem.Word())
            .Generate(200);
    }

    public void InitializeForgeSchema()
    {
        Units = new Faker<UnitInfo>()
            .RuleFor(info => info.Unit, faker => faker.Lorem.Sentence())
            .RuleFor(info => info.Label, faker => faker.Lorem.Word())
            .RuleFor(info => info.Value, faker => faker.Lorem.Word())
            .RuleFor(info => info.Class, faker => faker.Lorem.Sentence())
            .Generate(200);
    }

    async partial void OnSearchTextChanged(string value)
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
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var family in Units)
                if (family.Label.Contains(formattedText, StringComparison.OrdinalIgnoreCase) || family.Unit.Contains(formattedText, StringComparison.OrdinalIgnoreCase))
                    searchResults.Add(family);

            return searchResults;
        });
    }

    partial void OnUnitsChanged(List<UnitInfo> value)
    {
        FilteredUnits = value;
    }
}