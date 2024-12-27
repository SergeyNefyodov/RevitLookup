using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Tests.Unit.Data.ComponentModel;

public sealed class ResolveDescriptor : Descriptor, IDescriptorResolver
{
    public ResolveDescriptor()
    {
        Name = "Redirection";
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[]? parameters)
    {
        return target switch
        {
            nameof(ResolveObject.UnsupportedMethod) => ResolveUnsupportedMethod,
            nameof(ResolveObject.UnsupportedDescribedMethod) => ResolveUnsupportedDescribedMethod,
            nameof(ResolveObject.UnsupportedMultiMethod) => ResolveUnsupportedMultiMethod,
            _ => null
        };

        IVariant ResolveUnsupportedMethod()
        {
            return Variants.Value("Resolved");
        }

        IVariant ResolveUnsupportedDescribedMethod()
        {
            return Variants.Value("Resolved", "Value description");
        }

        IVariant ResolveUnsupportedMultiMethod()
        {
            return Variants.Values<string>(2)
                .Add("Resolved 1")
                .Add("Resolved 2", "Value description")
                .Consume();
        }
    }
}