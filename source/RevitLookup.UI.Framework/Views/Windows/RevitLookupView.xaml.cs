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
using Wpf.Ui.Appearance;

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

        InitializeComponent();

        intercomService.SetSharedHost(this);
        navigationService.SetNavigationControl(RootNavigation);
        dialogService.SetDialogHost(RootContentDialog);
        snackbarService.SetSnackbarPresenter(RootSnackbar);

        AddShortcuts();
        AddBadges();
        ApplyTheme();
        ApplyWindowSize();
    }

    private void AddBadges()
    {
        if (_updateService.NewVersion is null) return;
        if (_updateService.LocalFilePath is not null) return;

        UpdatesNotifier.Visibility = Visibility.Visible;
    }

    private void ApplyTheme()
    {
        WindowBackdropType = _settingsService.GeneralSettings.Background;
        RootNavigation.Transition = _settingsService.GeneralSettings.Transition;
        ApplicationThemeManager.Apply(_settingsService.GeneralSettings.Theme, _settingsService.GeneralSettings.Background);
    }

    private void ApplyWindowSize()
    {
        if (!_settingsService.GeneralSettings.UseSizeRestoring) return;

        if (_settingsService.GeneralSettings.WindowWidth >= MinWidth) Width = _settingsService.GeneralSettings.WindowWidth;
        if (_settingsService.GeneralSettings.WindowHeight >= MinHeight) Height = _settingsService.GeneralSettings.WindowHeight;

        EnableSizeTracking();
    }

    public void EnableSizeTracking()
    {
        SizeChanged += OnSizeChanged;
    }

    public void DisableSizeTracking()
    {
        SizeChanged -= OnSizeChanged;
    }

    private static void OnSizeChanged(object sender, SizeChangedEventArgs args)
    {
        var self = (RevitLookupView)sender;
        self._settingsService.GeneralSettings.WindowWidth = args.NewSize.Width;
        self._settingsService.GeneralSettings.WindowHeight = args.NewSize.Height;
    }
}