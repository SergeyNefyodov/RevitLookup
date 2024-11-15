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
using RevitLookup.Abstractions.Services;
using Wpf.Ui;

namespace RevitLookup.UI.Framework.Views.Windows;

public sealed partial class RevitLookupView
{
    private readonly IWindowIntercomService _intercomService;
    private readonly ISoftwareUpdateService _updateService;
    private readonly ISettingsService _settingsService;

    public RevitLookupView(
        INavigationService navigationService,
        IContentDialogService dialogService,
        ISnackbarService snackbarService,
        IWindowIntercomService intercomService,
        ISoftwareUpdateService updateService,
        ISettingsService settingsService)
    {
        _intercomService = intercomService;
        _updateService = updateService;
        _settingsService = settingsService;

        // RootNavigation.TransitionDuration = _settings.TransitionDuration;
        // WindowBackdropType = _settings.Background;

        InitializeComponent();
        AddShortcuts();
        AddBadges();

        intercomService.SetSharedHost(this);
        navigationService.SetNavigationControl(RootNavigation);
        dialogService.SetDialogHost(RootContentDialog);
        snackbarService.SetSnackbarPresenter(RootSnackbar);

        // ApplyTheme();
        // RestoreSize();
    }

    private void AddBadges()
    {
        if (_updateService.NewVersion is null) return;
        if (_updateService.LocalFilePath is not null) return;

        UpdatesNotifier.Visibility = Visibility.Visible;
    }
//     private void ApplyTheme()
//     {
//         if (_settings.Theme == ApplicationTheme.Auto)
//         {
// #if REVIT2024_OR_GREATER
//             RevitThemeWatcher.Watch(_settings.Background);
// #else
//             throw new NotSupportedException("Auto theme is not supported for current Revit version");
// #endif
//         }
//         else
//         {
//             ApplicationThemeManager.Apply(_settings.Theme, _settings.Background);
//         }
//     }

    // private void RestoreSize()
    // {
    //     if (!_settings.UseSizeRestoring) return;
    //
    //     if (_settings.WindowWidth >= MinWidth) Width = _settings.WindowWidth;
    //     if (_settings.WindowHeight >= MinHeight) Height = _settings.WindowHeight;
    //
    //     EnableSizeTracking();
    // }

    //
    // public void EnableSizeTracking()
    // {
    //     SizeChanged += OnSizeChanged;
    // }
    //
    // public void DisableSizeTracking()
    // {
    //     SizeChanged -= OnSizeChanged;
    // }
    //
    // private void OnSizeChanged(object sender, SizeChangedEventArgs args)
    // {
    //     _settings.WindowWidth = args.NewSize.Width;
    //     _settings.WindowHeight = args.NewSize.Height;
    // }
}