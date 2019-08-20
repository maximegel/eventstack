using EventStack.Domain;

namespace EventStack.Infrastructure.Testing.Doubles
{
    public interface IDummyAggregateRoot : IAggregateRoot
    {
        string Foo { get; }

        void UpdateFoo(string value);
    }
}