using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;

namespace LookupEngine.Tests.Unit.Data.ComponentModel;

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