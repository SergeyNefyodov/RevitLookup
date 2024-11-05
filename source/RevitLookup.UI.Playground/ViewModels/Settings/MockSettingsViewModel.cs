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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Settings;
using RevitLookup.UI.Framework.Views.Settings;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.ViewModels.Settings;

[UsedImplicitly]
public sealed partial class MockSettingsViewModel(IServiceProvider serviceProvider, INotificationService notificationService) : ObservableObject, ISettingsViewModel
{
    [ObservableProperty] private ApplicationTheme _theme;
    [ObservableProperty] private WindowBackdropType _background;

    [ObservableProperty] private bool _useTransition;
    [ObservableProperty] private bool _useHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring;
    [ObservableProperty] private bool _useModifyTab;

    public List<ApplicationTheme> Themes { get; } =
    [
        ApplicationTheme.Auto,
        ApplicationTheme.Light,
        ApplicationTheme.Dark,
        ApplicationTheme.HighContrast
    ];

    public List<WindowBackdropType> BackgroundEffects { get; } =
    [
        WindowBackdropType.None,
        WindowBackdropType.Acrylic,
        WindowBackdropType.Tabbed,
        WindowBackdropType.Mica
    ];

    [RelayCommand]
    private async Task ResetSettings()
    {
        var dialog = serviceProvider.GetRequiredService<ResetSettingsDialog>();
        var result = await dialog.ShowAsync();
        if (result != ContentDialogResult.Primary) return;

        notificationService.ShowSuccess("Reset was successful", "Some changes will be applied after closing the window");
        await Task.CompletedTask;
    }
}