using System;
using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain.Internal
{
    internal class GuardedWriteOnlyRepository<TAggregate, TId> : IWriteOnlyRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>
    {
        private readonly IWriteOnlyRepository<TAggregate, TId> _inner;

        public GuardedWriteOnlyRepository(IWriteOnlyRepository<TAggregate, TId> inner) => _inner = inner;

        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
            return _inner.DeleteAsync(aggregate, cancellationToken);
        }

        public async Task<Option<TAggregate>> FindAsync(TId id, CancellationToken cancellationToken = default)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            return await _inner.FindAsync(id, cancellationToken);
        }

        public Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate is null) throw new ArgumentNullException(nameof(aggregate));
            return _inner.SaveAsync(aggregate, cancellationToken);
        }
    }
}