using System.Reflection;
using LookupEngine.Formaters;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

public partial class LookupComposer
{
    private void DecomposeEvents(BindingFlags bindingFlags)
    {
        if (!_options.IncludeEvents) return;

        var members = MemberDeclaringType.GetEvents(bindingFlags);
        foreach (var member in members)
        {
            WriteDecompositionMember(ReflexionFormater.FormatTypeName(member.EventHandlerType ?? typeof(object)), member);
        }
    }
}