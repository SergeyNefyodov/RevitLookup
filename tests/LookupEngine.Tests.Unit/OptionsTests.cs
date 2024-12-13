using Bogus;
using LookupEngine.Abstractions.ComponentModel;
using LookupEngine.Tests.Unit.Data;

namespace LookupEngine.Tests.Unit;

public sealed class LookupEngineTests
{
    [Test]
    public async Task Decompose_NonZeroMembers()
    {
        //Arrange
        var data = new Faker().Random.String();

        //Act
        var defaultResult = LookupComposer.Decompose(data);

        //Assert
        await Assert.That(defaultResult.Members).IsNotEmpty();
    }

    [Test]
    public async Task Decompose_IncludingFields_NonZeroFields()
    {
        //Arrange
        var data = new PublicFieldsObject();
        var options = new DecomposeOptions
        {
            IncludeFields = true,
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingPrivate_NonZeroPrivateMembers()
    {
        //Arrange
        var data = new Faker().Random.String();
        var options = new DecomposeOptions
        {
            IncludePrivateMembers = true,
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingUnsupported_NonZeroUnsupported()
    {
        //Arrange
        var data = new Faker().Random.String();
        var options = new DecomposeOptions
        {
            IncludeUnsupported = true
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingRoot_NonZeroRootMembers()
    {
        //Arrange
        var data = new Faker().Random.String();
        var options = new DecomposeOptions
        {
            IncludeRoot = true
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingStatic_NonZeroStaticMembers()
    {
        //Arrange
        var data = new Faker().Date.Future();
        var options = new DecomposeOptions
        {
            IncludeStaticMembers = true
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingEvents_NonZeroEvents()
    {
        //Arrange
        var data = AppDomain.CurrentDomain;
        var options = new DecomposeOptions
        {
            IncludeEvents = true
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members.Count < comparableResult.Members.Count).IsTrue();
    }

    [Test]
    public async Task Decompose_IncludingRedirection_RedirectedToAnotherValue()
    {
        //Arrange
        var data = new RedirectionObject
        {
            Property = new RedirectionObject()
        };

        var options = new DecomposeOptions
        {
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectionObject => new RedirectionDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        await Assert.That(() => defaultResult.Members[0].Value.TypeName != comparableResult.Members[0].Value.TypeName).IsTrue();
    }
}