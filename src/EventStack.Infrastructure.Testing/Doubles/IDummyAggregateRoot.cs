using EventStack.Domain;

namespace EventStack.Infrastructure.Testing.Doubles
{
    public interface IDummyAggregateRoot : IAggregateRoot<int>
    {
        string Foo { get; }

        void UpdateFoo(string value);
    }
}