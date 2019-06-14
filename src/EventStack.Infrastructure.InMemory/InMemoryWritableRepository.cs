using System.Linq;
using EventStack.Domain;
using EventStack.Infrastructure.InMemory.Internal;

namespace EventStack.Infrastructure.InMemory
{
    public static class InMemoryWritableRepository
    {
        public static IWritableRepository<TEntity> Empty<TEntity>()
            where TEntity : class, IEntity =>
            From<TEntity>();

        public static IWritableRepository<TEntity> From<TEntity>(params TEntity[] entities)
            where TEntity : class, IEntity =>
            new InMemoryWritableRepository<TEntity>(entities.ToDictionary(entity => entity.Id))
                .RequireNonNullArguments();
    }
}