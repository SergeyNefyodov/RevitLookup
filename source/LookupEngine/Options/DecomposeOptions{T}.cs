using JetBrains.Annotations;

namespace LookupEngine.Options;

[PublicAPI]
public class DecomposeOptions<TContext> : DecomposeOptions
{
    public required TContext Context { get; set; }
}