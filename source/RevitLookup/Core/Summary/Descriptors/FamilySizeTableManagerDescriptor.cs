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

using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class FamilySizeTableManagerDescriptor(FamilySizeTableManager manager) : Descriptor, IDescriptorResolver
{
    public Func<IVariant>? Resolve(string target, ParameterInfo[] parameters)
    {
        return target switch
        {
            nameof(FamilySizeTableManager.GetSizeTable) => ResolveSizeTable,
            nameof(FamilySizeTableManager.HasSizeTable) => ResolveHasSizeTable,
            nameof(FamilySizeTableManager.GetFamilySizeTableManager) => ResolveGetFamilySizeTableManager,
            _ => null
        };

        IVariant ResolveSizeTable()
        {
            var names = manager.GetAllSizeTableNames();
            var variants = Variants.Values<FamilySizeTable>(names.Count);

            foreach (var name in names)
            {
                variants.Add(manager.GetSizeTable(name), name);
            }

            return variants.Consume();
        }

        IVariant ResolveHasSizeTable()
        {
            var names = manager.GetAllSizeTableNames();
            var variants = Variants.Values<bool>(names.Count);

            foreach (var name in names)
            {
                var result = manager.HasSizeTable(name);
                variants.Add(result, $"{name}: {result}");
            }

            return variants.Consume();
        }

        IVariant ResolveGetFamilySizeTableManager()
        {
            return Variants.Value(manager);
        }
    }

    // public void RegisterMenu(ContextMenu contextMenu)
    // {
    //     var context = (ISnoopViewModel) contextMenu.DataContext;
    //     var document = context.SnoopableObjects[0].Context;
    //     
    //     contextMenu.AddMenuItem("ExportMenuItem")
    //         .SetHeader("Export table")
    //         .SetAvailability(manager.GetAllSizeTableNames().Count > 0)
    //         .SetCommand(manager, async _ =>
    //         {
    //             try
    //             {
    //                 var dialog = new FamilySizeTableSelectDialog(context.ServiceProvider, document, manager);
    //                 await dialog.ShowExportDialogAsync();
    //             }
    //             catch (Exception exception)
    //             {
    //                 var logger = context.ServiceProvider.GetRequiredService<ILogger<ParameterDescriptor>>();
    //                 logger.LogError(exception, "Initialize FamilySizeTableExportDialog error");
    //             }
    //         });
    //     
    //     contextMenu.AddMenuItem("EditMenuItem")
    //         .SetHeader("Edit table")
    //         .SetAvailability(document.IsFamilyDocument && manager.GetAllSizeTableNames().Count > 0)
    //         .SetCommand(manager, async _ =>
    //         {
    //             try
    //             {
    //                 var dialog = new FamilySizeTableSelectDialog(context.ServiceProvider, document, manager);
    //                 await dialog.ShowEditDialogAsync();
    //             }
    //             catch (Exception exception)
    //             {
    //                 var logger = context.ServiceProvider.GetRequiredService<ILogger<FamilySizeTableDescriptor>>();
    //                 logger.LogError(exception, "Initialize FamilySizeTableSelectDialog error");
    //             }
    //         });
    // }
}