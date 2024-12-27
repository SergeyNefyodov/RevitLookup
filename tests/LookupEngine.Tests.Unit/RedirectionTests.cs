using LookupEngine.Descriptors;
using LookupEngine.Tests.Unit.Descriptors;
using LookupEngine.Tests.Unit.Objects;

namespace LookupEngine.Tests.Unit;

public sealed class RedirectionTests
{
    [Test]
    public async Task Decompose_IncludingRedirection_RedirectedToAnotherValue()
    {
        //Arrange
        var data = new RedirectableObject
        {
            Property = new RedirectableObject()
        };

        var options = new DecomposeOptions
        {
            TypeResolver = (obj, _) =>
            {
                return obj switch
                {
                    RedirectableObject => new RedirectionDescriptor(),
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
            await Assert.That(comparableResult.Members[0].AllocatedBytes).IsPositive();
            await Assert.That(comparableResult.Members[0].ComputationTime).IsPositive();
            await Assert.That(comparableResult.Members[0].Value.TypeName).IsNotEqualTo(defaultResult.Members[0].Value.TypeName);
        }
    }
}