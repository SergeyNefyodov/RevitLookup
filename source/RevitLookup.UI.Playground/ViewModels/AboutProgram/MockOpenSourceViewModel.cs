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

using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;
using RevitLookup.Abstractions.Models;
using RevitLookup.Abstractions.ViewModels.AboutProgram;

namespace RevitLookup.UI.Playground.ViewModels.AboutProgram;

[UsedImplicitly]
public sealed class MockOpenSourceViewModel : ObservableObject, IOpenSourceViewModel
{
    public List<OpenSourceSoftware> Software { get; } =
    [
        new()
        {
            SoftwareName = "CommunityToolkit.Mvvm",
            SoftwareUri = "https://github.com/CommunityToolkit/dotnet",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/CommunityToolkit/dotnet/blob/main/License.md"
        },
        new()
        {
            SoftwareName = "Microsoft.Extensions.Hosting",
            SoftwareUri = "https://github.com/dotnet/runtime",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/dotnet/runtime/blob/main/LICENSE.TXT"
        },
        new()
        {
            SoftwareName = "Nice3point.Revit.Api",
            SoftwareUri = "https://github.com/Nice3point/RevitApi",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitApi/blob/main/License.md"
        },
        new()
        {
            SoftwareName = "Nice3point.Revit.Extensions",
            SoftwareUri = "https://github.com/Nice3point/RevitExtensions",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitExtensions/blob/main/License.md"
        },
        new()
        {
            SoftwareName = "Nice3point.Revit.Templates",
            SoftwareUri = "https://github.com/Nice3point/RevitTemplates",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitTemplates/blob/main/License.md"
        },
        new()
        {
            SoftwareName = "Nice3point.Revit.Toolkit",
            SoftwareUri = "https://github.com/Nice3point/RevitToolkit",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Nice3point/RevitToolkit/blob/main/License.md"
        },
        new()
        {
            SoftwareName = "PolySharp",
            SoftwareUri = "https://github.com/Sergio0694/PolySharp",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/Sergio0694/PolySharp/blob/main/LICENSE"
        },
        new()
        {
            SoftwareName = "Serilog",
            SoftwareUri = "https://github.com/serilog/serilog",
            LicenseName = "Apache License 2.0",
            LicenseUri = "https://github.com/serilog/serilog/blob/dev/LICENSE"
        },
        new()
        {
            SoftwareName = "WPF-UI",
            SoftwareUri = "https://github.com/lepoco/wpfui",
            LicenseName = "MIT License",
            LicenseUri = "https://github.com/lepoco/wpfui/blob/main/LICENSE"
        }
    ];
}