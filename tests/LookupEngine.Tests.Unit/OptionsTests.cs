namespace LookupEngine.Tests.Unit;

public sealed class LookupEngineTests
{
    [Test]
    public async Task Decompose_DefaultOptions()
    {
        //Act
        var descriptors = LookupComposer.Decompose("Hello World");

        //Assert
        await Assert.That(descriptors.Members).IsNotEmpty();
    }

    [Test]
    public async Task Decompose_Include_Fields()
    {
        //Arrange
        var options = new DecomposeOptions
        {
            IncludeFields = true
        };

        //Act
        var defaultDescriptors = LookupComposer.Decompose("Hello World");
        var optionalDescriptors = LookupComposer.Decompose("Hello World", options);

        //Assert
        await Assert.That(() => optionalDescriptors.Members.Count > defaultDescriptors.Members.Count).ThrowsNothing();
    }

    [Test]
    public async Task Decompose_With_Redirection()
    {
        //Arrange
        var options = new DecomposeOptions
        {
            IncludeFields = true
        };

        //Act
        var defaultDescriptors = LookupComposer.Decompose(69);
        var optionalDescriptors = LookupComposer.Decompose(69, options);

        //Assert
        await Assert.That(optionalDescriptors.Members.Count(data => data.Value is string))
            .IsGreaterThan(defaultDescriptors.Members.Count(data => data.Value is string));
    }
}