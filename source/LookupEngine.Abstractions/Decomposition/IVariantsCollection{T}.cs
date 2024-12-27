namespace LookupEngine.Abstractions.Decomposition;

public interface IVariantsCollection<in T>
{
    IVariantsCollection<T> Add(T result);
    IVariantsCollection<T> Add(T result, string description);
    IVariant Consume();
}