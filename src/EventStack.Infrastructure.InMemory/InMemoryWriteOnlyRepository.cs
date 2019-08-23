using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryWriteOnlyRepository<TAggregate, TId> :
        IWriteOnlyRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>
    {
        private readonly string _collectionKey;
        private readonly InMemoryStorage _storage;

        private InMemoryWriteOnlyRepository(string collectionKey, InMemoryStorage storage)
        {
            _collectionKey = collectionKey;
            _storage = storage;
        }

        /// <inheritdoc />
        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            _storage.Remove(_collectionKey, aggregate);
            _storage.SaveChanges();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<Option<TAggregate>> FindAsync(TId id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_storage.Find<TAggregate, TId>(_collectionKey, id));

        /// <inheritdoc />
        public Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            _storage.AddOrUpdate(_collectionKey, aggregate);
            _storage.SaveChanges();
            return Task.CompletedTask;
        }

        public static IWriteOnlyRepository<TAggregate, TId> Create(string collectionKey, InMemoryStorage storage) =>
            new InMemoryWriteOnlyRepository<TAggregate, TId>(collectionKey, storage).UseGuardClauses();
    }
}