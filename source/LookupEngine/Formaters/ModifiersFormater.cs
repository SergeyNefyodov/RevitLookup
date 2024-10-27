using System.Reflection;
using LookupEngine.Abstractions.Enums;

namespace LookupEngine.Formaters;

internal static class ModifiersFormater
{
    internal static MemberAttributes FormatAttributes(MemberInfo member)
    {
        return member switch
        {
            MethodInfo info => MemberAttributes.Method.AddModifiers(info.Attributes),
            PropertyInfo info => MemberAttributes.Property.AddModifiers(info.CanRead ? info.GetMethod!.Attributes : info.SetMethod!.Attributes),
            FieldInfo info => MemberAttributes.Field.AddModifiers(info.Attributes),
            EventInfo info => MemberAttributes.Event.AddModifiers(info.AddMethod!.Attributes),
            _ => throw new ArgumentOutOfRangeException(nameof(member))
        };
    }

    private static MemberAttributes AddModifiers(this MemberAttributes attributes, MethodAttributes methodAttributes)
    {
        if ((methodAttributes & MethodAttributes.Static) != 0) attributes |= MemberAttributes.Static;
        if ((methodAttributes & MethodAttributes.Private) != 0) attributes |= MemberAttributes.Private;
        return attributes;
    }

    private static MemberAttributes AddModifiers(this MemberAttributes attributes, FieldAttributes fieldAttributes)
    {
        if ((fieldAttributes & FieldAttributes.Static) != 0) attributes |= MemberAttributes.Static;
        if ((fieldAttributes & FieldAttributes.Private) != 0) attributes |= MemberAttributes.Private;
        return attributes;
    }
}