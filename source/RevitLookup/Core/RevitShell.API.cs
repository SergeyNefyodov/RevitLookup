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
using System.Runtime.InteropServices;
using Autodesk.Revit.UI;

namespace RevitLookup.Core;

public static partial class RevitShell
{
    public static UIControlledApplication CreateUiControlledApplication()
    {
        return (UIControlledApplication) Activator.CreateInstance(
            typeof(UIControlledApplication),
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [Context.UiApplication],
            null)!;
    }

    private static string GetLabel(ForgeTypeId typeId, PropertyInfo property)
    {
        if (typeId.Empty()) return string.Empty;
        if (property.Name == nameof(SpecTypeId.Custom)) return string.Empty;

        var type = property.DeclaringType;
        while (type!.IsNested)
        {
            type = type.DeclaringType;
        }

        try
        {
            return type.Name switch
            {
                nameof(UnitTypeId) => typeId.ToUnitLabel(),
                nameof(SpecTypeId) => typeId.ToSpecLabel(),
                nameof(SymbolTypeId) => typeId.ToSymbolLabel(),
#if REVIT2022_OR_GREATER
                nameof(ParameterTypeId) => typeId.ToParameterLabel(),
                nameof(GroupTypeId) => typeId.ToGroupLabel(),
                nameof(DisciplineTypeId) => typeId.ToDisciplineLabel(),
#endif
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        catch
        {
            //Some parameter label thrown an exception
            return string.Empty;
        }
    }

    public static Parameter GetBuiltinParameter(BuiltInParameter builtInParameter)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var parameterType = typeof(Parameter);
        var assembly = Assembly.GetAssembly(parameterType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var parameterCtorType = parameterType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInParameter);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var parameter = (Parameter) parameterCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return parameter;
    }

    public static Category GetBuiltinCategory(BuiltInCategory builtInCategory)
    {
        const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var documentType = typeof(Document);
        var categoryType = typeof(Category);
        var assembly = Assembly.GetAssembly(categoryType)!;
        var aDocumentType = assembly.GetType("ADocument")!;
        var elementIdType = assembly.GetType("ElementId")!;
        var elementIdIdType = elementIdType.GetField("<alignment member>", bindingFlags)!;
        var getADocumentType = documentType.GetMethod("getADocument", bindingFlags)!;
        var categoryCtorType = categoryType.GetConstructor(bindingFlags, null, [aDocumentType.MakePointerType(), elementIdType.MakePointerType()], null)!;

        var elementId = Activator.CreateInstance(elementIdType)!;
        elementIdIdType.SetValue(elementId, builtInCategory);

        var handle = GCHandle.Alloc(elementId);
        var elementIdPointer = GCHandle.ToIntPtr(handle);
        Marshal.StructureToPtr(elementId, elementIdPointer, true);

        var category = (Category) categoryCtorType.Invoke([getADocumentType.Invoke(Context.ActiveDocument, null), elementIdPointer]);
        handle.Free();

        return category;
    }
}