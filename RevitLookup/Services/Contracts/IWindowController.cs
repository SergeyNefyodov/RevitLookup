﻿// Copyright 2003-2023 by Autodesk, Inc.
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
using RevitLookup.UI.Controls.Interfaces;

namespace RevitLookup.Services.Contracts;

public interface IWindowController
{
    /// <summary>
    /// Lets you attach the window that represents the <see cref="INavigation"/>.
    /// </summary>
    /// <param name="window">Instance of the <see cref="Window"/>.</param>
    void SetControlledWindow(Window window);

    /// <summary>
    /// Hide navigation window
    /// </summary>
    void Hide();

    /// <summary>
    /// Show navigation window
    /// </summary>
    void Show();
}