using EventStack.Domain;
using EventStack.Infrastructure.Internal;

namespace EventStack.Infrastructure
{
    public static class WritableRepositoryGuards
    {
        public static IWritableRepository<TEntity> RequireNonNullArguments<TEntity>(
            this IWritableRepository<TEntity> repository)
            where TEntity : class =>
            new NullsGuardedWritableRepository<TEntity>(repository);
    }
}