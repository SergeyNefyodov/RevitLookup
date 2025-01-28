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
using System.Windows.Media;
using RevitLookup.UI.Framework.Utils;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Views.Windows;

public sealed partial class RevitLookupView
{
    private void FixComponentsTheme()
    {
        RootNavigation.Loaded += OnNavigationScrollLoaded;
    }

    private void OnNavigationScrollLoaded(object sender, RoutedEventArgs args)
    {
        var contentPresenter = RootNavigation.FindVisualChild<NavigationViewContentPresenter>()!;
        contentPresenter.LoadCompleted += ContentPresenterOnContentRendered;
    }

    private void ContentPresenterOnContentRendered(object? sender, EventArgs e)
    {
        var contentPresenter = (NavigationViewContentPresenter) sender!;
        if (!contentPresenter.IsDynamicScrollViewerEnabled) return;

        if (VisualTreeHelper.GetChildrenCount(contentPresenter) == 0)
        {
            contentPresenter.ApplyTemplate();
        }

        var scrollViewer = (ScrollViewer) VisualTreeHelper.GetChild(contentPresenter, 0);
        _themeWatcherService.Watch(scrollViewer);
    }
}