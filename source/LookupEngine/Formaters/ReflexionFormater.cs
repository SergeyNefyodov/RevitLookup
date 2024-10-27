using System.Reflection;
using LookupEngine.Abstractions.Metadata;

namespace LookupEngine.Formaters;

public static class ReflexionFormater
{
    public static string FormatTypeName(Type? type)
    {
        if (type is null) return string.Empty;
        if (!type.IsGenericType) return type.Name;

        var typeName = type.Name;
        var apostropheIndex = typeName.IndexOf('`');
        if (apostropheIndex > 0) typeName = typeName[..apostropheIndex];
        typeName += "<";
        var genericArguments = type.GetGenericArguments();
        for (var i = 0; i < genericArguments.Length; i++)
        {
            typeName += FormatTypeName(genericArguments[i]);
            if (i < genericArguments.Length - 1) typeName += ", ";
        }

        typeName += ">";
        return typeName;
    }

    public static string? FormatTypeFullName(Type type)
    {
        var fullName = type.FullName;
        if (fullName is null) return null;

        return type.IsGenericType ? fullName[..fullName.IndexOf('[')] : fullName;
    }

    public static string FormatMemberName(MemberInfo member, ParameterInfo[]? types)
    {
        if (types is null) return member.Name;
        if (types.Length == 0) return member.Name;

        var parameterNames = types.Select(info =>
        {
            return info.ParameterType.IsByRef switch
            {
                true => $"ref {FormatTypeName(info.ParameterType).Replace("&", string.Empty)}",
                _ => FormatTypeName(info.ParameterType)
            };
        });

        var parameters = string.Join(", ", parameterNames);
        return $"{member.Name} ({parameters})";
    }

    internal static void FormatDefaultProperties(Type type, Descriptor descriptor)
    {
        descriptor.Type = FormatTypeName(type);
        descriptor.Name ??= descriptor.Type;
        descriptor.TypeFullName = FormatTypeFullName(type);
    }
}