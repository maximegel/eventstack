using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryWritableRepository<TEntity> :
        IWritableRepository<TEntity>,
        IUnitOfWorkParticipant
        where TEntity : class, IEntity
    {
        private readonly List<Action<IDictionary<object, TEntity>>> _pendingActions =
            new List<Action<IDictionary<object, TEntity>>>();

        private readonly IDictionary<object, TEntity> _storage;

        public InMemoryWritableRepository(IDictionary<object, TEntity> storage) => _storage = storage;

        /// <inheritdoc />
        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            lock (_storage)
            {
                // We try to execute all pending actions on a copy of the current storage before running them on the real storage
                // to make sure everything will work properly without error. This ensures that the transaction will be atomic. 
                _pendingActions.ForEach(action => action(new Dictionary<object, TEntity>(_storage)));

                _pendingActions.ForEach(action => action(_storage));
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void AddOrUpdate(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _pendingActions.Add(storage => storage[entity.Id] = entity);
        }

        /// <inheritdoc />
        public void Remove(TEntity entity)
        {
            entity = entity ?? throw new ArgumentNullException(nameof(entity));
            _pendingActions.Add(storage => storage.Remove(entity.Id));
        }

        /// <inheritdoc />
        public Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));
            return Task.FromResult(_storage.TryGetValue(id));
        }
    }
}