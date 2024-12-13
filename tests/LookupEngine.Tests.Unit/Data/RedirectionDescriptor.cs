using LookupEngine.Abstractions.ComponentModel;
using LookupEngine.Abstractions.Configuration;

namespace LookupEngine.Tests.Unit.Data;

public sealed class RedirectionDescriptor : Descriptor, IDescriptorRedirection
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