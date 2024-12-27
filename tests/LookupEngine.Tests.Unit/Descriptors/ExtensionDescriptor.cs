using LookupEngine.Abstractions.Configuration;
using LookupEngine.Abstractions.Decomposition;
using LookupEngine.Tests.Unit.Contexts;

namespace LookupEngine.Tests.Unit.Descriptors;

public sealed class ExtensionDescriptor : Descriptor, IDescriptorExtension, IDescriptorExtension<EngineContext>
{
    public void RegisterExtensions(IExtensionManager manager)
    {
        manager.Register("Extension", Extension);
        return;

        IVariant Extension()
        {
            return Variants.Value("Extended");
        }
    }

    public void RegisterExtensions(IExtensionManager<EngineContext> manager)
    {
        manager.Register("VersionExtension", VersionExtension);
        manager.Register("MetadataExtension", MetadataExtension);
        return;

        IVariant VersionExtension(EngineContext context)
        {
            return Variants.Value(context.Version);
        }

        IVariant MetadataExtension(EngineContext context)
        {
            return Variants.Value(context.Metadata);
        }
    }
}