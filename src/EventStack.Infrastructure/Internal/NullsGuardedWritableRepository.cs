using System;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using RailSharp;

namespace EventStack.Infrastructure.Internal
{
    internal class NullsGuardedWritableRepository<TEntity> : IWritableRepository<TEntity>
        where TEntity : class
    {
        private readonly IWritableRepository<TEntity> _inner;

        public NullsGuardedWritableRepository(IWritableRepository<TEntity> inner) => _inner = inner;

        public void AddOrUpdate(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _inner.AddOrUpdate(entity);
        }

        public void Remove(TEntity entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));
            _inner.Remove(entity);
        }

        public async Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id is null) throw new ArgumentNullException(nameof(id));
            return await _inner.TryFindAsync(id, cancellationToken);
        }
    }
}