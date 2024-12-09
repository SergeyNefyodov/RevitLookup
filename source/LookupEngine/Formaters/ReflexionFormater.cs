using System.Reflection;

namespace LookupEngine.Formaters;

public static class ReflexionFormater
{
    public static string FormatTypeName(Type type)
    {
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

    public static string FormatMemberName(MemberInfo member, ParameterInfo[]? parameters)
    {
        if (parameters is null) return member.Name;
        if (parameters.Length == 0) return member.Name;

        var formatedParameters = parameters.Select(info =>
        {
            return info.ParameterType.IsByRef switch
            {
                true => $"ref {FormatTypeName(info.ParameterType).Replace("&", string.Empty)}",
                false => FormatTypeName(info.ParameterType)
            };
        });

        return $"{member.Name} ({string.Join(", ", formatedParameters)})";
    }
}