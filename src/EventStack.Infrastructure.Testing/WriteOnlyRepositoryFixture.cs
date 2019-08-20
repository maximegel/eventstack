using System.Collections.Generic;
using EventStack.Domain;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.Testing
{
    public abstract class WriteOnlyRepositoryFixture<TAggreagte>
        where TAggreagte : class, IDummyAggregateRoot
    {
        public abstract IWriteOnlyRepository<TAggreagte> Repository { get; }

        public abstract IUnitOfWork UnitOfWork { get; }

        public abstract void Seed(IEnumerable<TAggreagte> aggregates);
    }
}