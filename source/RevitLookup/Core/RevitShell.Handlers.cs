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
using Nice3point.Revit.Toolkit.External.Handlers;
using RevitLookup.Abstractions.ObservableModels.Decomposition;

namespace RevitLookup.Core;

public static partial class RevitShell
{
    private static ActionEventHandler? _actionEventHandler;
    private static AsyncEventHandler? _asyncEventHandler;
    private static AsyncEventHandler<ObservableDecomposedObject>? _asyncObjectHandler;
    private static AsyncEventHandler<List<ObservableDecomposedObject>>? _asyncObjectsHandler;
    private static AsyncEventHandler<List<ObservableDecomposedMember>>? _asyncMembersHandler;
    private static AsyncEventHandler<IEnumerable>? _asyncCollectionHandler;

    public static ActionEventHandler ActionEventHandler
    {
        get => _actionEventHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _actionEventHandler = value;
    }

    public static AsyncEventHandler AsyncEventHandler
    {
        get => _asyncEventHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncEventHandler = value;
    }

    public static AsyncEventHandler<ObservableDecomposedObject> AsyncObjectHandler
    {
        get => _asyncObjectHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncObjectHandler = value;
    }

    public static AsyncEventHandler<List<ObservableDecomposedObject>> AsyncObjectsHandler
    {
        get => _asyncObjectsHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncObjectsHandler = value;
    }

    public static AsyncEventHandler<List<ObservableDecomposedMember>> AsyncMembersHandler
    {
        get => _asyncMembersHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncMembersHandler = value;
    }

    public static AsyncEventHandler<IEnumerable> AsyncCollectionHandler
    {
        get => _asyncCollectionHandler ?? throw new InvalidOperationException("The Handler was never set.");
        private set => _asyncCollectionHandler = value;
    }

    public static void RegisterHandlers()
    {
        ActionEventHandler = new ActionEventHandler();
        AsyncEventHandler = new AsyncEventHandler();
        AsyncObjectHandler = new AsyncEventHandler<ObservableDecomposedObject>();
        AsyncObjectsHandler = new AsyncEventHandler<List<ObservableDecomposedObject>>();
        AsyncMembersHandler = new AsyncEventHandler<List<ObservableDecomposedMember>>();
        AsyncCollectionHandler = new AsyncEventHandler<IEnumerable>();
    }
}