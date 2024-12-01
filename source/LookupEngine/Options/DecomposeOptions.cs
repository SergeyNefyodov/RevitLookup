using JetBrains.Annotations;
using LookupEngine.Abstractions.ComponentModel;

// ReSharper disable once CheckNamespace
namespace LookupEngine;

[PublicAPI]
public sealed class DecomposeOptions
{
    private Func<object?, Type?, Descriptor>? _typeResolver;

    public Func<object?, Type?, Descriptor> TypeResolver
    {
        get { return _typeResolver ??= DefaultResolveMap; }
        set => _typeResolver = value;
    }

    public bool IncludeRoot { get; set; }
    public bool IncludeFields { get; set; }
    public bool IncludeEvents { get; set; }
    public bool IncludeUnsupported { get; set; }
    public bool IgnorePrivateMembers { get; set; } = true;
    public bool IgnoreStaticMembers { get; set; } = true;
    public bool EnableExtensions { get; set; }

    public static DecomposeOptions Default => new();

    private static Descriptor DefaultResolveMap(object? obj, Type? type)
    {
        return obj switch
        {
            bool value when type is null || type == typeof(bool) => new BooleanDescriptor(value),
            Exception value when type is null || type == typeof(Exception) => new ExceptionDescriptor(value),
            _ => new ObjectDescriptor(obj)
        };
    }
}