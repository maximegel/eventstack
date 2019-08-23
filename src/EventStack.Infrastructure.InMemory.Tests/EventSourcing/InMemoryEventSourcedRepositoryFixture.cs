using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Infrastructure.EventSourcing;
using EventStack.Infrastructure.InMemory.EventSourcing;
using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.EventSourcing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests.EventSourcing
{
    public class InMemoryEventSourcedRepositoryFixture : WriteOnlyRepositoryFixture<DummyEventSourcedAggregateRoot>
    {
        public InMemoryEventSourcedRepositoryFixture()
            : base(RepositoryFactory(), AggregateFactory, Seeder)
        {
        }

        private static DummyEventSourcedAggregateRoot AggregateFactory(int id) =>
            new DummyEventSourcedAggregateRoot(id);

        private static IWriteOnlyRepository<DummyEventSourcedAggregateRoot, int> RepositoryFactory() =>
            EventSourcedRepositoryBuilder.For<DummyEventSourcedAggregateRoot, int>()
                .UseEventStore(new InMemoryEventStore(InMemoryStorage.Empty))
                .Build();

        private static void Seeder(
            IWriteOnlyRepository<DummyEventSourcedAggregateRoot, int> repository,
            IEnumerable<DummyEventSourcedAggregateRoot> aggregates) =>
            Task.WhenAll(aggregates.Select(aggregate => repository.SaveAsync(aggregate))).Wait();
    }
}