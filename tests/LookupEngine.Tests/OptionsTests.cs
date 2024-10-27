using LookupEngine.Options;

namespace LookupEngine.Tests;

public sealed class LookupEngineTests
{
    [Test]
    public async Task Decompose_DefaultOptions()
    {
        var descriptors = LookupComposer.Decompose("Hello World");
        await Assert.That(descriptors).IsNotEmpty();
    }

    [Test]
    public async Task Decompose_With_Fields()
    {
        var options = new DecomposeOptions
        {
            IgnoreFields = false
        };

        var defaultDescriptors = LookupComposer.Decompose("Hello World");
        var optionalDescriptors = LookupComposer.Decompose("Hello World", options);
        await Assert.That(() => optionalDescriptors.Count > defaultDescriptors.Count).ThrowsNothing();
    }
}