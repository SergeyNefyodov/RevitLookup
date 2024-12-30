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

using Autodesk.Revit.UI;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.Commands;
using RevitLookup.Core;
using UIFramework;
using RibbonExtensions = RevitLookup.Utils.Ribbon.RibbonExtensions;

namespace RevitLookup.Services.Application;

public sealed class RevitRibbonService(ISettingsService settingsService)
{
    private readonly List<RibbonPanel> _createdPanels = new(2);

    public void CreateRibbon()
    {
        RevitShell.ActionEventHandler.Raise(_ =>
        {
            if (_createdPanels.Count == 0)
            {
                CreatePanels();
                return;
            }

            RemovePanels();
            CreatePanels();
            ShortcutsHelper.LoadCommands();
        });
    }

    private void CreatePanels()
    {
        var application = Context.UiControlledApplication;
        var addinsPanel = RibbonExtensions.CreatePanel(application, "Revit Lookup");
        var pullButton = addinsPanel.AddPullDownButton("RevitLookupButton", "RevitLookup");
        pullButton.SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png");
        pullButton.SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        pullButton.AddPushButton<DashboardCommand>("Dashboard");
        if (!settingsService.GeneralSettings.UseModifyTab)
        {
            pullButton.AddPushButton<SnoopSelectionCommand>("Snoop Selection");
        }

        pullButton.AddPushButton<SnoopViewCommand>("Snoop Active view");
        pullButton.AddPushButton<SnoopDocumentCommand>("Snoop Document");
        pullButton.AddPushButton<SnoopDatabaseCommand>("Snoop Database");
        pullButton.AddPushButton<SnoopFaceCommand>("Snoop Face");
        pullButton.AddPushButton<SnoopEdgeCommand>("Snoop Edge");
        pullButton.AddPushButton<SnoopPointCommand>("Snoop Point");
        pullButton.AddPushButton<SnoopLinkedElementCommand>("Snoop Linked element");
        pullButton.AddPushButton<SearchElementsCommand>("Search Elements");
        pullButton.AddPushButton<EventMonitorCommand>("Event monitor");

        _createdPanels.Add(addinsPanel);
        if (!settingsService.GeneralSettings.UseModifyTab) return;

        var modifyPanel = RibbonExtensions.CreatePanel(application, "Revit Lookup", "Modify");
        modifyPanel.AddPushButton<SnoopSelectionCommand>(" Snoop \nSelection")
            .SetImage("/RevitLookup;component/Resources/Images/RibbonIcon16.png")
            .SetLargeImage("/RevitLookup;component/Resources/Images/RibbonIcon32.png");

        _createdPanels.Add(modifyPanel);
    }

    private void RemovePanels()
    {
        foreach (var ribbonPanel in _createdPanels)
        {
            RibbonExtensions.RemovePanel(ribbonPanel);
        }

        _createdPanels.Clear();
    }
}