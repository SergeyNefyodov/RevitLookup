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

using System.Windows.Interop;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Settings;
using RevitLookup.UI.Framework.Views.Settings;
using Wpf.Ui;
using Wpf.Ui.Animations;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.ViewModels.Settings;

[UsedImplicitly]
public sealed partial class MockSettingsViewModel : ObservableObject, ISettingsViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingsService _settingsService;
    private readonly IWindowIntercomService _intercomService;

    [ObservableProperty] private ApplicationTheme _theme;
    [ObservableProperty] private WindowBackdropType _background;

    [ObservableProperty] private bool _useTransition;
    [ObservableProperty] private bool _useHardwareRendering;
    [ObservableProperty] private bool _useSizeRestoring;
    [ObservableProperty] private bool _useModifyTab;

    public MockSettingsViewModel(
        IServiceProvider serviceProvider,
        INavigationService navigationService,
        INotificationService notificationService,
        ISettingsService settingsService,
        IWindowIntercomService intercomService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _settingsService = settingsService;
        _intercomService = intercomService;

        ApplySettings();
    }

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
        try
        {
            var dialog = _serviceProvider.GetRequiredService<ResetSettingsDialog>();
            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            _settingsService.ResetSettings();
            ApplySettings();
            _notificationService.ShowSuccess("Reset settings", "Settings successfully reset to default");
        }
        catch (Exception exception)
        {
            _notificationService.ShowError("Reset settings error", exception);
        }
    }

    partial void OnThemeChanged(ApplicationTheme value)
    {
        _settingsService.GeneralSettings.Theme = value;
        ApplicationThemeManager.Apply(value, Background);
    }

    partial void OnBackgroundChanged(WindowBackdropType value)
    {
        _settingsService.GeneralSettings.Background = value;
        ApplicationThemeManager.Apply(Theme, value);
    }

    partial void OnUseTransitionChanged(bool value)
    {
        var navigationControl = _navigationService.GetNavigationControl();
        var transition = _settingsService.GeneralSettings.Transition = value
            ? (Transition)NavigationView.TransitionProperty.DefaultMetadata.DefaultValue
            : Transition.None;

        _settingsService.GeneralSettings.Transition = transition;
        navigationControl.Transition = transition;
    }

    partial void OnUseHardwareRenderingChanged(bool value)
    {
        _settingsService.GeneralSettings.UseHardwareRendering = value;
        RenderOptions.ProcessRenderMode = value ? RenderMode.Default : RenderMode.SoftwareOnly;
    }

    partial void OnUseSizeRestoringChanged(bool value)
    {
        _settingsService.GeneralSettings.UseSizeRestoring = value;
    }

    partial void OnUseModifyTabChanged(bool value)
    {
        _settingsService.GeneralSettings.UseModifyTab = value;
    }

    private void ApplySettings()
    {
        Theme = _settingsService.GeneralSettings.Theme;
        Background = _settingsService.GeneralSettings.Background;
        UseTransition = _settingsService.GeneralSettings.Transition != Transition.None;
        UseHardwareRendering = _settingsService.GeneralSettings.UseHardwareRendering;
        UseSizeRestoring = _settingsService.GeneralSettings.UseSizeRestoring;
        UseModifyTab = _settingsService.GeneralSettings.UseModifyTab;
    }
}