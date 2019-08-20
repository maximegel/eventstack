using System.Collections.Generic;
using EventStack.Domain;
using EventStack.Infrastructure.Testing;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.InMemory.Tests
{
    public class InMemoryWriteOnlyRepositoryFixture : WriteOnlyRepositoryFixture<DummyAggregateRoot>
    {
        public InMemoryWriteOnlyRepositoryFixture()
        {
            var storage = InMemoryStorage.Empty;
            Repository = new InMemoryWriteOnlyRepository<DummyAggregateRoot>(storage);
            UnitOfWork = new InMemoryUnitOfWork(storage);
        }

        /// <inheritdoc />
        public override IWriteOnlyRepository<DummyAggregateRoot> Repository { get; }

        /// <inheritdoc />
        public override IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc />
        public override void Seed(IEnumerable<DummyAggregateRoot> aggregates)
        {
            Repository.AddOrUpdateRange(aggregates);
            UnitOfWork.CommitAsync().Wait();
        }
    }
}