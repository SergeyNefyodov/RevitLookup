using Bogus;
using LookupEngine.Abstractions.Descriptors.System;
using LookupEngine.Tests.Unit.Data;
using LookupEngine.Tests.Unit.Data.ComponentModel;

namespace LookupEngine.Tests.Unit;

public sealed class LookupEngineTests
{
    [Test]
    public async Task Decompose_DefaultOptions_HasMembers()
    {
        //Arrange
        var data = new Faker().Random.String();

        //Act
        var defaultResult = LookupComposer.Decompose(data);

        //Assert
        await Assert.That(defaultResult.Members).IsNotEmpty();
    }

    [Test]
    public async Task Decompose_IncludingFields_HasFields()
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
    public async Task Decompose_IncludingPrivate_HasPrivateMembers()
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
    public async Task Decompose_IncludingUnsupported_HasUnsupported()
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
    public async Task Decompose_IncludingRoot_HasRootMembers()
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
    public async Task Decompose_IncludingStatic_HasStaticMembers()
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
    public async Task Decompose_IncludingEvents_HasEvents()
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
        using (Assert.Multiple())
        {
            await Assert.That(defaultResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(() => defaultResult.Members[0].Value.TypeName != comparableResult.Members[0].Value.TypeName).IsTrue();
        }
    }

    [Test]
    public async Task Decompose_IncludingUnresolvedData_ResolvedData()
    {
        //Arrange
        var data = new ResolveObject();
        var options = new DecomposeOptions
        {
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    ResolveObject => new ResolveDescriptor(),
                    _ => new ObjectDescriptor(obj)
                };
            }
        };

        //Act
        var defaultResult = LookupComposer.Decompose(data);
        var comparableResult = LookupComposer.Decompose(data, options);

        //Assert
        using (Assert.Multiple())
        {
            await Assert.That(defaultResult.Members).IsEmpty();
            await Assert.That(comparableResult.Members).IsNotEmpty();
            await Assert.That(comparableResult.Members[1].Value.Description).IsNotNullOrEmpty();
        }
    }
}