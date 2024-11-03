namespace LookupEngine;

public sealed class Redirect<TInput, TOutput>(Func<TInput, TOutput> converter) : IRedirect
{
    public Func<TInput, TOutput> Converter { get; } = converter;
}