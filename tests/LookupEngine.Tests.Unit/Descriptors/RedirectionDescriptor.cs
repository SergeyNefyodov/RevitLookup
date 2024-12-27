using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Tests.Unit.Descriptors;

public sealed class RedirectionDescriptor : Descriptor, IDescriptorRedirector
{
    public RedirectionDescriptor()
    {
        Name = "Redirection";
    }

    public bool TryRedirect(string targetMember, out object result)
    {
        result = true;
        return true;
    }
}