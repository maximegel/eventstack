using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests
{
    public class InMemoryWriteOnlyRepositoryFixture : WriteOnlyRepositoryFixture<DummyAggregateRoot>
    {
        public InMemoryWriteOnlyRepositoryFixture()
            : base(RepositoryFactory(), AggregateFactory, Seeder)
        {
        }

        private static DummyAggregateRoot AggregateFactory(int id) =>
            new DummyAggregateRoot(id);

        private static IWriteOnlyRepository<DummyAggregateRoot, int> RepositoryFactory()
        {
            var storage = InMemoryStorage.Empty;
            return InMemoryWriteOnlyRepository<DummyAggregateRoot, int>.Create("dummies", storage);
        }

        private static void Seeder(
            IWriteOnlyRepository<DummyAggregateRoot, int> repository,
            IEnumerable<DummyAggregateRoot> aggregates) =>
            Task.WhenAll(aggregates.Select(aggregate => repository.SaveAsync(aggregate))).Wait();
    }
}