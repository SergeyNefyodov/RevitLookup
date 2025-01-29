using LookupEngine.Abstractions.Decomposition.Containers;

namespace LookupEngine.Abstractions.Decomposition;

/// <summary>
///     A factory for <see cref="IVariant"/>.
/// </summary>
public static class Variants
{
    /// <summary>
    ///     Creates a variant collection with a single value
    /// </summary>
    /// <returns>A variant collection containing the specified value</returns>
    public static IVariant Value(object? value)
    {
        return new Variant(value);
    }

    public static IVariant Value(object? value, string description)
    {
        return new Variant(value, description);
    }

    public static IVariantsCollection<T> Values<T>(int capacity)
    {
        return new Variants<T>(capacity);
    }

    /// <summary>
    ///     Creates an empty variant collection
    /// </summary>
    /// <returns>An empty variant collection</returns>
    /// <remarks>An empty collection is returned when there are no solutions for a member</remarks>
    public static IVariant Empty<T>()
    {
        return new Variants<T>(0);
    }

    /// <summary>
    ///     A variant that disables the member calculation
    /// </summary>
    public static IVariant Disabled()
    {
        return new Variant(new InvalidOperationException("Member execution disabled"));
    }
}