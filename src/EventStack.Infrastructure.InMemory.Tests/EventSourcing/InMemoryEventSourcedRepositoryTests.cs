using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.EventSourcing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests.EventSourcing
{
    public class InMemoryEventSourcedRepositoryTests :
        WriteOnlyRepositoryTests<InMemoryEventSourcedRepositoryFixture, DummyEventSourcedAggregateRoot>
    {
        /// <inheritdoc />
        public InMemoryEventSourcedRepositoryTests(InMemoryEventSourcedRepositoryFixture fixture)
            : base(fixture)
        {
        }

        /// <inheritdoc />
        protected override DummyEventSourcedAggregateRoot CreateAggregate(int id) =>
            new DummyEventSourcedAggregateRoot(id);
    }
}