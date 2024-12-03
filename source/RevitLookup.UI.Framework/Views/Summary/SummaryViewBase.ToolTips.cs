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

using System.Text;
using System.Windows;
using LookupEngine.Abstractions.Enums;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.UI.Framework.Views.Summary;

public partial class SummaryViewBase
{
    /// <summary>
    ///     Create tree view tooltips
    /// </summary>
    private static void CreateTreeTooltip(ObservableDecomposedObject decomposedObject, FrameworkElement row)
    {
        if (row.ToolTip is not null) return;

        row.ToolTip = new StringBuilder()
            .Append("Name: ")
            .AppendLine(decomposedObject.Name)
            .Append("Type: ")
            .AppendLine(decomposedObject.TypeName)
            .Append("Full type: ")
            .AppendLine(decomposedObject.TypeFullName)
            .Append("Members: ")
            .Append(decomposedObject.Members.Count)
            .ToString();
    }

    /// <summary>
    ///     Create tree view tooltips
    /// </summary>
    private static void CreateTreeTooltip(ObservableDecomposedObjectsGroup decomposedGroup, FrameworkElement row)
    {
        if (row.ToolTip is not null) return;

        row.ToolTip = new StringBuilder()
            .Append("Type: ")
            .AppendLine(decomposedGroup.GroupName)
            .Append("Items: ")
            .Append(decomposedGroup.GroupItems.Count)
            .ToString();
    }

    /// <summary>
    ///     Create data grid tooltips
    /// </summary>
    private static void CreateGridRowTooltip(ObservableDecomposedMember member, FrameworkElement row)
    {
        if (row.ToolTip is not null) return;

        var builder = new StringBuilder();

        if ((member.MemberAttributes & MemberAttributes.Private) != 0) builder.Append("Private ");
        if ((member.MemberAttributes & MemberAttributes.Static) != 0) builder.Append("Static ");
        if ((member.MemberAttributes & MemberAttributes.Property) != 0) builder.Append("Property: ");
        if ((member.MemberAttributes & MemberAttributes.Extension) != 0) builder.Append("Extension: ");
        if ((member.MemberAttributes & MemberAttributes.Method) != 0) builder.Append("Method: ");
        if ((member.MemberAttributes & MemberAttributes.Event) != 0) builder.Append("Event: ");
        if ((member.MemberAttributes & MemberAttributes.Field) != 0) builder.Append("Field: ");

        builder.AppendLine(member.Name)
            .Append("Type: ")
            .AppendLine(member.Value.TypeName)
            .Append("Full type: ")
            .AppendLine(member.Value.TypeFullName)
            .Append("Value: ")
            .Append(member.Value.Name);

        if (member.Description is not null)
        {
            builder.AppendLine()
                .Append("Description: ")
                .Append(member.Description);
        }

        if (member.ComputationTime > 0)
        {
            builder.AppendLine()
                .Append("Time: ")
                .Append(member.ComputationTime)
                .Append(" ms");
        }

        if (member.AllocatedBytes > 0)
        {
            builder.AppendLine()
                .Append("Allocated: ")
                .Append(member.AllocatedBytes)
                .Append(" bytes");
        }

        row.ToolTip = builder.ToString();
    }
}