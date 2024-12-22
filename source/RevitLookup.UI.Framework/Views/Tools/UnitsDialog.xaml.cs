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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RevitLookup.Abstractions.Models.Tools;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.ViewModels.Tools;
using RevitLookup.UI.Framework.Extensions;
using RevitLookup.UI.Framework.Views.Summary;
using Wpf.Ui;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.UI.Framework.Views.Tools;

public sealed partial class UnitsDialog
{
    private readonly IUnitsViewModel _viewModel;
    private readonly INavigationService _navigationService;

    public UnitsDialog(
        IContentDialogService dialogService,
        IUnitsViewModel viewModel,
        INavigationService navigationService,
        IThemeWatcherService themeWatcherService)
        : base(dialogService.GetDialogHost())
    {
        _viewModel = viewModel;
        _navigationService = navigationService;

        themeWatcherService.Watch(this);

        DataContext = _viewModel;
        InitializeComponent();
    }

    public async Task ShowParametersDialogAsync()
    {
        _viewModel.InitializeParameters();

        Title = "BuiltIn Parameters";
        DialogMaxWidth = 1000;

        await ShowAsync();
    }

    public async Task ShowCategoriesDialogAsync()
    {
        _viewModel.InitializeCategories();

        Title = "BuiltIn Categories";
        DialogMaxWidth = 600;

        await ShowAsync();
    }

    public async Task ShowForgeSchemaDialogAsync()
    {
        _viewModel.InitializeForgeSchema();

        ClassColumn.Visibility = Visibility.Visible;
        Title = "Forge Schema";
        DialogMaxWidth = 1100;

        await ShowAsync();
    }

    private void OnMouseEnter(object sender, RoutedEventArgs routedEventArgs)
    {
        var element = (FrameworkElement) sender;
        var unitInfo = (UnitInfo) element.DataContext;
        CreateTreeContextMenu(unitInfo, element);
    }

    private void CreateTreeContextMenu(UnitInfo info, FrameworkElement row)
    {
        var contextMenu = new ContextMenu
        {
            Resources = UiApplication.Current.Resources,
            PlacementTarget = row
        };

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy unit")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Unit))
            .SetShortcut(ModifierKeys.Control, Key.C);

        contextMenu.AddMenuItem("CopyMenuItem")
            .SetHeader("Copy label")
            .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Label));

        if (info.Class is not null)
        {
            contextMenu.AddMenuItem("CopyMenuItem")
                .SetHeader("Copy class")
                .SetCommand(info, unitInfo => Clipboard.SetDataObject(unitInfo.Class!))
                .SetShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.C);
        }

        contextMenu.AddMenuItem("SnoopMenuItem")
            .SetHeader("Snoop")
            .SetCommand(info, async unitInfo =>
            {
                Hide();
                await _viewModel.DecomposeAsync(unitInfo);
                _navigationService.Navigate(typeof(DecompositionSummaryPage));
            });

        row.ContextMenu = contextMenu;
    }
}