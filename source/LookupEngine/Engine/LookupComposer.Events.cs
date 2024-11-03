using System.Reflection;
using LookupEngine.Formaters;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public sealed partial class LookupComposer
{
    private void DecomposeEvents(BindingFlags bindingFlags)
    {
        if (_options.IncludeEvents) return;

        var members = Subtype.GetEvents(bindingFlags);
        foreach (var member in members)
        {
            WriteDecompositionResult(ReflexionFormater.FormatTypeName(member.EventHandlerType), member);
        }
    }
}