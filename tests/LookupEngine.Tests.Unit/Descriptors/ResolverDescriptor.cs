using System.Reflection;
using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Tests.Unit.Contexts;
using LookupEngine.Tests.Unit.Objects;

namespace LookupEngine.Tests.Unit.Descriptors;

public sealed class ResolverDescriptor : Descriptor, IDescriptorResolver, IDescriptorResolver<EngineContext>
{
    public ResolverDescriptor()
    {
        Name = "Redirection";
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[]? parameters)
    {
        return target switch
        {
            nameof(ResolvableObject.UnsupportedMethod) => ResolveUnsupportedMethod,
            nameof(ResolvableObject.UnsupportedDescribedMethod) => ResolveUnsupportedDescribedMethod,
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
    }

    public Func<IVariant>? Resolve(string target, ParameterInfo[]? parameters, EngineContext context)
    {
        return target switch
        {
            nameof(ResolvableObject.UnsupportedMultiMethod) => ResolveUnsupportedMultiMethod,
            _ => null
        };

        IVariant ResolveUnsupportedMultiMethod()
        {
            return Variants.Values<string>(2)
                .Add("Resolved 1")
                .Add("Resolved 2", "Value description")
                .Consume();
        }
    }
}