using EventStack.Domain;
using EventStack.Infrastructure.Internal;

namespace EventStack.Infrastructure
{
    public static class WriteOnlyRepositoryDecorators
    {
        public static IWriteOnlyRepository<TAggregate> UseGuardClauses<TAggregate>(
            this IWriteOnlyRepository<TAggregate> repository)
            where TAggregate : class, IAggregateRoot =>
            new GuardedWriteOnlyRepository<TAggregate>(repository);
    }
}