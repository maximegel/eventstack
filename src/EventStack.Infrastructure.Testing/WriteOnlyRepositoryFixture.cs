using System;
using System.Collections.Generic;
using EventStack.Domain;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.Testing
{
    public abstract class WriteOnlyRepositoryFixture<TAggreagte>
        where TAggreagte : class, IDummyAggregateRoot
    {
        protected WriteOnlyRepositoryFixture(
            IWriteOnlyRepository<TAggreagte, int> repository,
            Func<int, TAggreagte> aggregateFactory,
            Action<IWriteOnlyRepository<TAggreagte, int>, IEnumerable<TAggreagte>> seeder)
        {
            Repository = repository;
            CreateAggregate = aggregateFactory;
            seeder(
                Repository,
                new[]
                {
                    // XXX: Id=1 is updated by a test bellow.
                    CreateAggregate(1),
                    // XXX: Id=2 is added by a test bellow.
                    // XXX: Id=4 is removed by a test bellow.
                    CreateAggregate(4),
                    CreateAggregate(6)
                    // XXX: Id=7 should not exist.
                    // XXX: Id=8 should not exist.
                });
        }

        public Func<int, TAggreagte> CreateAggregate { get; }

        public IWriteOnlyRepository<TAggreagte, int> Repository { get; }
    }
}