using System.Collections;
using System.Reflection;
using LookupEngine.Abstractions.Collections;
using LookupEngine.Abstractions.Configuration;

namespace LookupEngine.Abstractions.ComponentModel;

public sealed class EnumerableDescriptor : Descriptor, IDescriptorEnumerator, IDescriptorResolver
{
    public EnumerableDescriptor(IEnumerable value)
    {
        Enumerator = value.GetEnumerator();

        //Checking types to reduce memory allocation when creating an iterator and increase performance
        IsEmpty = value switch
        {
            ICollection enumerable => enumerable.Count == 0,
            _ => !Enumerator.MoveNext()
        };

        if (Enumerator is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    public IEnumerator Enumerator { get; }
    public bool IsEmpty { get; }

    public Func<IVariants>? Resolve(string target, ParameterInfo[]? parameters)
    {
        return target switch
        {
            nameof(IEnumerable.GetEnumerator) => ResolveGetEnumerator,
            _ => null
        };

        IVariants ResolveGetEnumerator()
        {
            return Variants.Empty<IEnumerator>();
        }
    }
}