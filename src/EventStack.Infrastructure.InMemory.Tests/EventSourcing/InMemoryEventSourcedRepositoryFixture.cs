using System.Collections.Generic;
using System.Linq;
using EventStack.Domain;
using EventStack.Domain.EventSourcing;
using EventStack.Infrastructure.EventSourcing;
using EventStack.Infrastructure.InMemory.EventSourcing;
using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.EventSourcing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests.EventSourcing
{
    public class InMemoryEventSourcedRepositoryFixture : WriteOnlyRepositoryFixture<DummyEventSourcedAggregateRoot>
    {
        public InMemoryEventSourcedRepositoryFixture()
        {
            var storage = InMemoryStorage.Empty;
            var eventStore = new InMemoryEventStore<IDomainEvent>(storage);
            Repository = EventSourcedRepositoryBuilder<DummyEventSourcedAggregateRoot>.For(eventStore).Build();
            UnitOfWork = new InMemoryUnitOfWork(storage);
        }

        /// <inheritdoc />
        public override IWriteOnlyRepository<DummyEventSourcedAggregateRoot> Repository { get; }

        /// <inheritdoc />
        public override IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc />
        public override void Seed(IEnumerable<DummyEventSourcedAggregateRoot> aggregates)
        {
            Repository.AddOrUpdateRange(aggregates);
            UnitOfWork.CommitAsync().Wait();
        }
    }
}