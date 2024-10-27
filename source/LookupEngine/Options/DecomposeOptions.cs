using LookupEngine.Abstractions.Metadata;

namespace LookupEngine.Options;

public sealed class DecomposeOptions
{
    private Func<object?, Type?, Descriptor>? _typeResolver;

    public Func<object?, Type?, Descriptor> TypeResolver
    {
        get { return _typeResolver ??= DefaultResolveMap; }
        set => _typeResolver = value;
    }

    public bool IgnoreFields { get; set; } = true;
    public bool IgnoreRoot { get; set; } = false;
    public bool IgnorePrivate { get; set; } = true;
    public bool IgnoreStatic { get; set; } = false;
    public bool IgnoreEvents { get; set; } = false;
    public bool IgnoreUnsupported { get; set; } = true;
    public bool HandleExtensions { get; set; } = false;

    public static DecomposeOptions Default => new();

    private static Descriptor DefaultResolveMap(object? obj, Type? type)
    {
        return obj switch
        {
            null when type is null => new ObjectDescriptor(),
            _ => new ObjectDescriptor(obj),
        };
    }
}