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

namespace RevitLookup.UI.Framework.Views.Summary;

public partial class SummaryViewBase
{
    // /// <summary>
    // ///     Data grid row style selector
    // /// </summary>
    // private void SelectDataGridRowStyle(DataGridRow row)
    // {
    //     var member = (ObservableDecomposedMember)row.DataContext;
    //
    //     var styleName = member.Value.RawValue switch
    //                     {
    //                         Exception => "ExceptionDataGridRowStyle",
    //                         ICollection { Count: > 0 } => "EnumerableDataGridRowStyle",
    //                         _ => null
    //                     }
    //                     ??
    //                     member.Value.Descriptor switch
    //                     {
    //                         IDescriptorEnumerator { IsEmpty: false } => "HandleDataGridRowStyle",
    //                         IDescriptorCollector => "HandleDataGridRowStyle",
    //                         _ => "DefaultLookupDataGridRowStyle"
    //                     };
    //
    //     row.Style = (Style)FindResource(styleName);
    // }
}

// public sealed class TreeViewItemTemplateSelector : DataTemplateSelector
// {
//     /// <summary>
//     ///     Tree view row style selector
//     /// </summary>
//     public override DataTemplate SelectTemplate(object item, DependencyObject container)
//     {
//         if (item is null) return null;
//
//         var presenter = (FrameworkElement) container;
//         var templateName = item switch
//         {
//             CollectionViewGroup => "DefaultLookupTreeViewGroupTemplate",
//             SnoopableObject snoopableObject => snoopableObject.Descriptor switch
//             {
//                 ColorDescriptor => "TreeViewColorItemTemplate",
//                 ColorMediaDescriptor => "TreeViewColorItemTemplate",
//                 _ => "DefaultLookupTreeViewItemTemplate"
//             },
//
//             _ => "DefaultLookupTreeViewItemTemplate"
//         };
//
//         return (DataTemplate) presenter.FindResource(templateName);
//     }