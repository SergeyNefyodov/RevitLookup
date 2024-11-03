namespace LookupEngine.Tests;

public sealed class LookupEngineTests
{
    [Test]
    public async Task Decompose_DefaultOptions()
    {
        //Act
        var descriptors = LookupComposer.Decompose("Hello World");

        //Assert
        await Assert.That(descriptors).IsNotEmpty();
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
        await Assert.That(() => optionalDescriptors.Count > defaultDescriptors.Count).ThrowsNothing();
    }

    [Test]
    public async Task Decompose_With_Redirection()
    {
        //Arrange
        var options = new DecomposeOptions
        {
            RedirectMap =
            [
                new Redirect<int, string>(i => "d")
            ]
        };

        //Act
        var defaultDescriptors = LookupComposer.Decompose(69);
        var optionalDescriptors = LookupComposer.Decompose(69, options);

        //Assert
        await Assert.That(optionalDescriptors.Count(data => data.Value is string))
            .IsGreaterThan(defaultDescriptors.Count(data => data.Value is string));
    }
}