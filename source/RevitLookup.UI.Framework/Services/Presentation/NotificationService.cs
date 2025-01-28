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
using RevitLookup.Abstractions.Services.Presentation;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Services.Presentation;

public sealed class NotificationService(ISnackbarService snackbarService, IWindowIntercomService intercomService) : INotificationService
{
    private Action? _pendingNotifications;

    public void ShowSuccess(string title, string message)
    {
        if (intercomService.Dispatcher.CheckAccess())
        {
            PushSuccessMessage(title, message);
        }
        else
        {
            intercomService.Dispatcher.Invoke(() => PushSuccessMessage(title, message));
        }
    }

    public void ShowWarning(string title, string message)
    {
        if (intercomService.Dispatcher.CheckAccess())
        {
            PushWarningMessage(title, message);
        }
        else
        {
            intercomService.Dispatcher.Invoke(() => PushWarningMessage(title, message));
        }
    }

    public void ShowError(string title, string message)
    {
        if (intercomService.Dispatcher.CheckAccess())
        {
            PushErrorMessage(title, message);
        }
        else
        {
            intercomService.Dispatcher.Invoke(() => PushErrorMessage(title, message));
        }
    }

    public void ShowError(string title, Exception exception)
    {
        if (intercomService.Dispatcher.CheckAccess())
        {
            PushErrorMessage(title, exception.Message);
        }
        else
        {
            intercomService.Dispatcher.Invoke(() => PushErrorMessage(title, exception.Message));
        }
    }

    private void PushSuccessMessage(string title, string message)
    {
        var host = intercomService.GetHost();
        if (!host.IsLoaded)
        {
            if (_pendingNotifications is null) host.Loaded += ShowPendingNotifications;
            _pendingNotifications += () => ShowSuccessBar(title, message);
        }
        else
        {
            ShowSuccessBar(title, message);
        }
    }

    private void PushWarningMessage(string title, string message)
    {
        var host = intercomService.GetHost();
        if (!host.IsLoaded)
        {
            if (_pendingNotifications is null) host.Loaded += ShowPendingNotifications;
            _pendingNotifications += () => ShowWarningBar(title, message);
        }
        else
        {
            ShowWarningBar(title, message);
        }
    }

    private void PushErrorMessage(string title, string message)
    {
        var host = intercomService.GetHost();
        if (!host.IsLoaded)
        {
            if (_pendingNotifications is null) host.Loaded += ShowPendingNotifications;
            _pendingNotifications += () => ShowErrorBar(title, message);
        }
        else
        {
            ShowErrorBar(title, message);
        }
    }

    private void ShowSuccessBar(string title, string message)
    {
        snackbarService.Show(
            title,
            message,
            ControlAppearance.Success,
            new SymbolIcon(SymbolRegular.ChatWarning24, 24),
            snackbarService.DefaultTimeOut);
    }

    private void ShowWarningBar(string title, string message)
    {
        snackbarService.Show(
            title,
            message,
            ControlAppearance.Caution,
            new SymbolIcon(SymbolRegular.Warning24, 24),
            snackbarService.DefaultTimeOut);
    }

    private void ShowErrorBar(string title, string message)
    {
        snackbarService.Show(
            title,
            message,
            ControlAppearance.Danger,
            new SymbolIcon(SymbolRegular.ErrorCircle24, 24),
            snackbarService.DefaultTimeOut);
    }

    private void ShowPendingNotifications(object sender, RoutedEventArgs args)
    {
        var host = intercomService.GetHost();
        host.Loaded -= ShowPendingNotifications;
        if (_pendingNotifications is null) return;

        _pendingNotifications.Invoke();
        _pendingNotifications = null;
    }
}