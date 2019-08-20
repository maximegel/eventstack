using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryWriteOnlyRepository<TAggregate> :
        IWriteOnlyRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        private readonly object _collectionKey = typeof(TAggregate);
        private readonly InMemoryStorage _storage;

        public InMemoryWriteOnlyRepository(InMemoryStorage storage) => _storage = storage;

        /// <inheritdoc />
        public void AddOrUpdate(TAggregate aggregate) => _storage.AddOrUpdate(_collectionKey, aggregate);

        /// <inheritdoc />
        public void Remove(TAggregate aggregate) => _storage.Remove(_collectionKey, aggregate);

        /// <inheritdoc />
        public Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_storage.TryFind<TAggregate>(_collectionKey, id));
    }
}