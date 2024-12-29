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

using System.Globalization;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace RevitLookup.Core.Summary.Descriptors;

public sealed class SolidDescriptor : Descriptor, IDescriptorExtension
{
    private readonly Solid _solid;

    public SolidDescriptor(Solid solid)
    {
        _solid = solid;
        Name = $"{solid.Volume.ToString(CultureInfo.InvariantCulture)} ft³";
    }

    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register(nameof(SolidUtils.SplitVolumes), () => Variants.Value(SolidUtils.SplitVolumes(_solid)));
        manager.Register(nameof(SolidUtils.IsValidForTessellation), () => Variants.Value(SolidUtils.IsValidForTessellation(_solid)));
    }

    //     public void RegisterMenu(ContextMenu contextMenu)

    //     {

// #if REVIT2023_OR_GREATER

//         contextMenu.AddMenuItem("SelectMenuItem")

//             .SetCommand(_solid, solid =>

//             {

//                 if (Context.ActiveUiDocument is null) return;

//

//                 RevitShell.ActionEventHandler.Raise(_ =>

//                 {

//                     var references = solid.Faces

//                         .Cast<Face>()

//                         .Select(face => face.Reference)

//                         .Where(reference => reference is not null)

//                         .ToList();

//

//                     if (references.Count == 0) return;

//

//                     Context.ActiveUiDocument.Selection.SetReferences(references);

//                 });

//             })

//             .SetShortcut(Key.F6);

//

//         contextMenu.AddMenuItem("ShowMenuItem")

//             .SetCommand(_solid, solid =>

//             {

//                 if (Context.ActiveUiDocument is null) return;

//

//                 RevitShell.ActionEventHandler.Raise(_ =>

//                 {

//                     var references = solid.Faces

//                         .Cast<Face>()

//                         .Select(face => face.Reference)

//                         .Where(reference => reference is not null)

//                         .ToList();

//

//                     if (references.Count == 0) return;

//

//                     var element = references[0].ElementId.ToElement(Context.ActiveDocument);

//                     if (element is not null) Context.ActiveUiDocument.ShowElements(element);

//                     Context.ActiveUiDocument.Selection.SetReferences(references);

//                 });

//             })

//             .SetShortcut(Key.F7);

// #endif

//

//         contextMenu.AddMenuItem("VisualizeMenuItem")

//             .SetAvailability(_solid.IsValidForTessellation())

//             .SetCommand(_solid, async solid =>

//             {

//                 if (Context.ActiveUiDocument is null) return;

//

//                 var context = (ISnoopViewModel) contextMenu.DataContext;

//

//                 try

//                 {

//                     var dialog = context.ServiceProvider.GetRequiredService<SolidVisualizationDialog>();

//                     await dialog.ShowDialogAsync(solid);

//                 }

//                 catch (Exception exception)

//                 {

//                     var logger = context.ServiceProvider.GetRequiredService<ILogger<SolidDescriptor>>();

//                     logger.LogError(exception, "VisualizationDialog error");

//                 }

//             })

//             .SetShortcut(Key.F8);

//     }
}