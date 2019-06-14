using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.InMemory.Internal
{
    internal class InMemoryWritableRepository<TEntity> :
        IWritableRepository<TEntity>,
        IUnitOfWorkParticipant
        where TEntity : class, IEntity
    {
        private readonly List<Action<IDictionary<string, TEntity>>> _unsavedOperations =
            new List<Action<IDictionary<string, TEntity>>>();

        private readonly IDictionary<string, TEntity> _storage;

        public InMemoryWritableRepository(IDictionary<string, TEntity> storage) => _storage = storage;

        /// <inheritdoc />
        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            lock (_storage)
            {
                // We try to execute all pending actions on a copy of the current storage before running them on the real storage
                // to make sure everything will work properly without error. This ensures that the transaction will be atomic. 
                _unsavedOperations.ForEach(action => action(new Dictionary<string, TEntity>(_storage)));

                _unsavedOperations.ForEach(action => action(_storage));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void AddOrUpdate(TEntity entity) => _unsavedOperations.Add(storage => storage[entity.Id] = entity);

        /// <inheritdoc />
        public void Remove(TEntity entity) => _unsavedOperations.Add(storage => storage.Remove(entity.Id));

        /// <inheritdoc />
        public Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_storage.TryGetValue(id.ToString()));
    }
}