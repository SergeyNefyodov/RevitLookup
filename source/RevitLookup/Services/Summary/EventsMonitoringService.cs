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

using System.Diagnostics;
using System.Reflection;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Models.EventArgs;
using RevitLookup.Core;

namespace RevitLookup.Services.Summary;

public sealed class EventsMonitoringService(ILogger<EventsMonitoringService> logger)
{
    private readonly Dictionary<EventInfo, Delegate> _handlersMap = new(16);

    private readonly Assembly[] _assemblies = AppDomain.CurrentDomain
        .GetAssemblies()
        .Where(assembly =>
        {
            var name = assembly.GetName().Name;
            return name is "RevitAPI" or "RevitAPIUI";
        })
        .Take(2)
        .ToArray();

    private readonly List<string> _denyList =
    [
        nameof(UIApplication.Idling),
        nameof(Autodesk.Revit.ApplicationServices.Application.ProgressChanged)
    ];

    public void Subscribe()
    {
        RevitShell.ActionEventHandler.Raise(Subscribe);
    }

    public void Unsubscribe()
    {
        RevitShell.ActionEventHandler.Raise(Unsubscribe);
    }

    private void Subscribe(UIApplication uiApplication)
    {
        if (_handlersMap.Count > 0) return;

        foreach (var dll in _assemblies)
        foreach (var type in dll.GetTypes())
        foreach (var eventInfo in type.GetEvents())
        {
            if (_denyList.Contains(eventInfo.Name)) continue;

            var targets = FindValidTargets(eventInfo.ReflectedType);
            if (targets.Length == 0)
            {
                logger.LogDebug("Missing target: {EventType}.{EventName}", eventInfo.ReflectedType, eventInfo.Name);
                break;
            }

            var methodInfo = GetType().GetMethod(nameof(OnHandlingEvent), BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)!;
            var eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType!, this, methodInfo);

            foreach (var target in targets)
            {
                eventInfo.AddEventHandler(target, eventHandler);
            }

            _handlersMap.Add(eventInfo, eventHandler);
            logger.LogDebug("Observing: {EventType}.{EventName}", eventInfo.ReflectedType, eventInfo.Name);
        }
    }

    private void Unsubscribe(UIApplication uiApplication)
    {
        foreach (var eventInfo in _handlersMap)
        {
            var targets = FindValidTargets(eventInfo.Key.ReflectedType);
            foreach (var target in targets)
            {
                eventInfo.Key.RemoveEventHandler(target, eventInfo.Value);
            }
        }

        _handlersMap.Clear();
    }

    private static object[] FindValidTargets(Type? targetType)
    {
        if (targetType == typeof(Document)) return Context.Application.Documents.Cast<object>().ToArray();
        if (targetType == typeof(Autodesk.Revit.ApplicationServices.Application)) return [Context.Application];
        if (targetType == typeof(UIApplication)) return [Context.UiApplication];

        return [];
    }

    public void OnHandlingEvent(object sender, EventArgs args)
    {
        var stackTrace = new StackTrace();
        var stackFrames = stackTrace.GetFrames()!;
        var eventName = stackFrames[1].GetMethod()!.Name;

        EventInvoked?.Invoke(sender, new EventInfoArgs
        {
            EventName = eventName.Replace(nameof(EventHandler), ""),
            Arguments = args
        });
    }

    public event EventHandler<EventInfoArgs>? EventInvoked;
}