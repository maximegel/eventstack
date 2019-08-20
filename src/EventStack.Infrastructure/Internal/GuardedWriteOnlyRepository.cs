using System;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.Internal
{
    internal class GuardedWriteOnlyRepository<TAggregate> : IWriteOnlyRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        private readonly IWriteOnlyRepository<TAggregate> _inner;

        public GuardedWriteOnlyRepository(IWriteOnlyRepository<TAggregate> inner) => _inner = inner;

        public void AddOrUpdate(TAggregate aggregate)
        {
            if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
            _inner.AddOrUpdate(aggregate);
        }

        public void Remove(TAggregate aggregate)
        {
            if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
            _inner.Remove(aggregate);
        }

        public async Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            return await _inner.TryFindAsync(id, cancellationToken);
        }
    }
}