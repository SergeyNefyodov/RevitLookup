using System.Reflection;
using LookupEngine.Abstractions.Metadata;
using LookupEngine.Formaters;

namespace LookupEngine;

public sealed partial class LookupComposer
{
    private void DecomposeEvents(Type inputType, Descriptor? descriptor, BindingFlags bindingFlags)
    {
        if (_options.IgnoreEvents) return;

        var members = inputType.GetEvents(bindingFlags);
        foreach (var member in members)
        {
            WriteDescriptor(ReflexionFormater.FormatTypeName(member.EventHandlerType), inputType, member);
        }
    }
}