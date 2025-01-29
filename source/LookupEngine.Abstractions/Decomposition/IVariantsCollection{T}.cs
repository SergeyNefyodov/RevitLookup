namespace LookupEngine.Abstractions.Decomposition;

public interface IVariantsCollection<in T> : IVariantsCollection
{
    IVariantsCollection<T> Add(T? result);
    IVariantsCollection<T> Add(T? result, string description);
}