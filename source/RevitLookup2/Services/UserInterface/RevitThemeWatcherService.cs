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

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using RevitLookup.Abstractions.Services;
using RevitLookup2.Core;
using Wpf.Ui.Appearance;

namespace RevitLookup2.Services.UserInterface;

public sealed class RevitThemeWatcherService(ISettingsService settingsService, IWindowIntercomService intercomService)
{
    private bool _isWatching;

    public void Watch()
    {
        var theme = settingsService.GeneralSettings.Theme;
#if REVIT2024_OR_GREATER
        if (settingsService.GeneralSettings.Theme == ApplicationTheme.Auto)
        {
            theme = GetRevitTheme();

            if (!_isWatching)
            {
                RevitShell.ActionEventHandler.Raise(_ => Context.UiApplication.ThemeChanged += ObserveThemeChanged);
                _isWatching = true;
            }
        }
#endif
        ApplicationThemeManager.Apply(theme, settingsService.GeneralSettings.Background);
    }

    public void Unwatch()
    {
#if REVIT2024_OR_GREATER
        if (!_isWatching) return;

        RevitShell.ActionEventHandler.Raise(_ => Context.UiApplication.ThemeChanged -= ObserveThemeChanged);
        _isWatching = false;
#endif
    }

    private void ObserveThemeChanged(object? sender, ThemeChangedEventArgs args)
    {
#if REVIT2024_OR_GREATER
        if (args.ThemeChangedType != ThemeType.UITheme) return;

        var theme = GetRevitTheme();
        intercomService.Dispatcher.Invoke(() =>
        {
            foreach (var window in intercomService.OpenedWindows)
            {
                ApplicationThemeManager.Apply(theme, settingsService.GeneralSettings.Background);
            }
        });
#endif
    }
#if REVIT2024_OR_GREATER

    private static ApplicationTheme GetRevitTheme()
    {
        return UIThemeManager.CurrentTheme switch
        {
            UITheme.Light => ApplicationTheme.Light,
            UITheme.Dark => ApplicationTheme.Dark,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
#endif
}