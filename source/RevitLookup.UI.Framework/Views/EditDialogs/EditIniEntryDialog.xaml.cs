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

using RevitLookup.Abstractions.ObservableModels.Entries;
using RevitLookup.Abstractions.Services.Appearance;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Framework.Views.EditDialogs;

public sealed partial class EditSettingsEntryDialog
{
    private ObservableIniEntry? _entry;

    public EditSettingsEntryDialog(
        IContentDialogService dialogService,
        IThemeWatcherService themeWatcherService)
        : base(dialogService.GetDialogHost())
    {
        themeWatcherService.Watch(this);
        InitializeComponent();
    }

    public ObservableIniEntry Entry
    {
        get => _entry ?? throw new InvalidOperationException("Entry was never set");
        private set => _entry = value;
    }

    public async Task<ContentDialogResult> ShowCreateDialogAsync(ObservableIniEntry? selectedEntry)
    {
        Title = "Create the entry";
        PrimaryButtonText = "Create";

        Entry = new ObservableIniEntry
        {
            IsActive = true
        };

        if (selectedEntry is not null)
        {
            Entry.Category = selectedEntry.Category;
        }

        DataContext = Entry;
        return await ShowAsync();
    }

    public async Task<ContentDialogResult> ShowUpdateDialogAsync(ObservableIniEntry entry)
    {
        Title = "Update the entry";
        PrimaryButtonText = "Update";

        Entry = entry;
        DataContext = entry;
        return await ShowAsync();
    }

    protected override void OnButtonClick(ContentDialogButton button)
    {
        if (button == ContentDialogButton.Primary)
        {
            Entry.Validate();
            if (Entry.HasErrors)
            {
                return;
            }
        }

        base.OnButtonClick(button);
    }
}