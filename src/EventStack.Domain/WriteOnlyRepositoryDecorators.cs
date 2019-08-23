using EventStack.Domain.Internal;

namespace EventStack.Domain
{
    public static class WriteOnlyRepositoryDecorators
    {
        public static IWriteOnlyRepository<TAggregate, TId> UseGuardClauses<TAggregate, TId>(
            this IWriteOnlyRepository<TAggregate, TId> repository)
            where TAggregate : class, IAggregateRoot<TId> =>
            new GuardedWriteOnlyRepository<TAggregate, TId>(repository);
    }
}