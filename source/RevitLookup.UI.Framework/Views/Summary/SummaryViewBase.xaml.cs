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

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using RevitLookup.Abstractions.ObservableModels.Decomposition;
using RevitLookup.Abstractions.Services;
using RevitLookup.Abstractions.ViewModels.Summary;
using RevitLookup.UI.Framework.Utils;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;
using DataGrid = System.Windows.Controls.DataGrid;
using TreeView = Wpf.Ui.Controls.TreeView;
using TreeViewItem = System.Windows.Controls.TreeViewItem;
using Visibility = System.Windows.Visibility;

namespace RevitLookup.UI.Framework.Views.Summary;

public partial class SummaryViewBase : Page, INavigableView<ISnoopSummaryViewModel>
{
    private readonly ISettingsService _settingsService;
    private readonly IWindowIntercomService _intercomService;

    protected SummaryViewBase(ISettingsService settingsService, IWindowIntercomService intercomService)
    {
        _settingsService = settingsService;
        _intercomService = intercomService;

        AddShortcuts();
    }

    public required UIElement SearchBoxControl { get; init; }
    public required TreeView TreeViewControl { get; init; }
    public required DataGrid DataGridControl { get; init; }
    public required ISnoopSummaryViewModel ViewModel { get; init; }

    protected void InitializeControls()
    {
        InitializeTreeView(TreeViewControl);
        InitializeDataGrid(DataGridControl);
    }

    /// <summary>
    ///     Tree view initialization
    /// </summary>
    private void InitializeTreeView(TreeView control)
    {
        control.SelectedItemChanged += OnTreeItemSelected;
        control.ItemsSourceChanged += OnTreeSourceChanged;
        control.MouseMove += OnPresenterCursorInteracted;
        control.ItemContainerGenerator.StatusChanged += OnTreeViewItemGenerated;

        if (control.ItemsSource is not null) OnTreeSourceChanged(control, control.ItemsSource);
    }

    /// <summary>
    ///     Tree view source changed handled. Setup action after the setting source
    /// </summary>
    private void OnTreeSourceChanged(object? sender, IEnumerable enumerable)
    {
        var self = (FrameworkElement)sender!;

        if (self.IsLoaded)
        {
            ExpandFirstTreeGroup();
            return;
        }

        self.Loaded += OnLoaded;
        return;

        void OnLoaded(object nestedSender, RoutedEventArgs args)
        {
            var nestedSelf = (FrameworkElement)nestedSender;
            nestedSelf.Loaded -= OnLoaded;
            ExpandFirstTreeGroup();
        }
    }

    /// <summary>
    ///     Expand the first tree view group after setting source
    /// </summary>
    private async void ExpandFirstTreeGroup()
    {
        try
        {
            // Await Frame transition. GetMembers freezes the thread and breaks the animation
            var transitionDuration = (int)NavigationView.TransitionDurationProperty.DefaultMetadata.DefaultValue;
            await Task.Delay(transitionDuration);

            //3 is optimal groups count for expanding
            if (TreeViewControl.Items.Count > 3) return;

            var rootItem = (TreeViewItem?)TreeViewControl.GetItemAtIndex(0);
            if (rootItem is null) return;

            var nestedItem = (TreeViewItem?)rootItem.GetItemAtIndex(0);
            if (nestedItem is null) return;

            nestedItem.IsSelected = true;
        }
        catch
        {
            // ignored
        }
    }

    /// <summary>
    ///     Handle tree view item loaded
    /// </summary>
    /// <remarks>
    ///     TreeView item customization after loading
    /// </remarks>
    private void OnTreeViewItemGenerated(object? sender, EventArgs _)
    {
        var generator = (ItemContainerGenerator)sender!;
        if (generator.Status == GeneratorStatus.ContainersGenerated)
        {
            foreach (var item in generator.Items)
            {
                var treeItem = (ItemsControl)generator.ContainerFromItem(item);
                if (treeItem is null) continue;

                treeItem.Loaded -= OnTreeItemLoaded;
                // treeItem.PreviewMouseLeftButtonUp -= OnTreeItemClicked;

                treeItem.Loaded += OnTreeItemLoaded;
                // treeItem.PreviewMouseLeftButtonUp += OnTreeItemClicked;

                if (treeItem.Items.Count > 0)
                {
                    treeItem.ItemContainerGenerator.StatusChanged -= OnTreeViewItemGenerated;
                    treeItem.ItemContainerGenerator.StatusChanged += OnTreeViewItemGenerated;
                }
            }
        }
    }

    /// <summary>
    ///     Create tree view tooltips, menus
    /// </summary>
    private void OnTreeItemLoaded(object? sender, RoutedEventArgs args)
    {
        var element = (FrameworkElement)sender!;
        switch (element.DataContext)
        {
            case ObservableDecomposedObjectsGroup decomposedGroup:
                CreateTreeTooltip(decomposedGroup, element);
                // CreateTreeContextMenu(context.Descriptor, element);
                break;
            case ObservableDecomposedObject decomposedObject:
                CreateTreeTooltip(decomposedObject, element);
                // CreateTreeContextMenu(context.Descriptor, element);
                break;
        }
    }

    /// <summary>
    ///     Handle data grid reference changed event
    /// </summary>
    /// <remarks>
    ///     Data grid initialization, validation
    /// </remarks>
    private void InitializeDataGrid(DataGrid control)
    {
        ApplyGrouping(control);
        ValidateTimeColumn(control);
        ValidateAllocatedColumn(control);
        // CreateGridContextMenu(dataGrid);
        control.LoadingRow += OnGridRowLoading;
        control.MouseMove += OnPresenterCursorInteracted;
    }

    /// <summary>
    ///     Set DataGrid grouping rules
    /// </summary>
    /// <param name="dataGrid"></param>
    private void ApplyGrouping(DataGrid dataGrid)
    {
        // dataGrid.Items.SortDescriptions.Clear();
        // dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(ObservableDecomposedMember.Depth), ListSortDirection.Descending));
        // dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(ObservableDecomposedMember.MemberAttributes), ListSortDirection.Ascending));
        // dataGrid.Items.SortDescriptions.Add(new SortDescription(nameof(ObservableDecomposedMember.Name), ListSortDirection.Ascending));

        dataGrid.Items.GroupDescriptions!.Clear();
        dataGrid.Items.GroupDescriptions.Add(new PropertyGroupDescription(nameof(ObservableDecomposedMember.DeclaringTypeName)));
    }

    // <summary>
    //     Handle data grid row loading event
    // </summary>
    // <remarks>
    //     Select row style
    // </remarks>
    protected void OnGridRowLoading(object? sender, DataGridRowEventArgs args)
    {
        var row = args.Row;
        row.Loaded += OnGridRowLoaded;
        // row.PreviewMouseLeftButtonUp += OnGridRowClicked;
        // SelectDataGridRowStyle(row);
    }

    /// <summary>
    ///     Handle data grid row loaded event
    /// </summary>
    /// <remarks>
    ///     Create tooltips, context menu
    /// </remarks>
    protected void OnGridRowLoaded(object sender, RoutedEventArgs args)
    {
        var element = (FrameworkElement)sender;
        var member = (ObservableDecomposedMember)element.DataContext;
        CreateGridRowTooltip(member, element);
        // CreateGridRowContextMenu(member, element);
    }

    /// <summary>
    ///     Show/hide time column
    /// </summary>
    private void ValidateTimeColumn(DataGrid control)
    {
        control.Columns[2].Visibility = _settingsService.GeneralSettings.ShowTimeColumn ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    ///     Show/hide allocated column
    /// </summary>
    private void ValidateAllocatedColumn(DataGrid control)
    {
        control.Columns[3].Visibility = _settingsService.GeneralSettings.ShowMemoryColumn ? Visibility.Visible : Visibility.Collapsed;
    }
}