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

using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Tools;
using RevitLookup.UI.Framework.Views.Summary;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Views.Tools;

public sealed partial class SearchElementsDialog
{
    private readonly ISearchElementsViewModel _viewModel;
    private readonly INavigationService _navigationService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<SearchElementsDialog> _logger;

    public SearchElementsDialog(
        IContentDialogService dialogService,
        ISearchElementsViewModel viewModel,
        INavigationService navigationService,
        INotificationService notificationService,
        ILogger<SearchElementsDialog> logger)
        : base(dialogService.GetDialogHost())
    {
        _viewModel = viewModel;
        _navigationService = navigationService;
        _notificationService = notificationService;
        _logger = logger;

        DataContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnButtonClick(ContentDialogButton button)
    {
        try
        {
            if (button == ContentDialogButton.Primary)
            {
                var success = await _viewModel.SearchElementsAsync();
                if (!success)
                {
                    return;
                }

                _navigationService.Navigate(typeof(DecompositionSummaryPage));
            }

            base.OnButtonClick(button);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while searching elements");
            _notificationService.ShowError("Search error", exception.Message);
        }
    }
}