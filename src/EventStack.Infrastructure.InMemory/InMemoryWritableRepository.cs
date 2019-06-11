using System.Collections.Generic;
using System.Linq;
using EventStack.Domain;

namespace EventStack.Infrastructure.InMemory
{
    public static class InMemoryWritableRepository
    {
        public static InMemoryWritableRepository<TEntity> From<TEntity>(params TEntity[] entities)
            where TEntity : class, IEntity =>
            new InMemoryWritableRepository<TEntity>(entities.ToDictionary(entity => entity.Id));

        public static InMemoryWritableRepository<TEntity> Empty<TEntity>()
            where TEntity : class, IEntity =>
            new InMemoryWritableRepository<TEntity>(new Dictionary<object, TEntity>());
    }
}