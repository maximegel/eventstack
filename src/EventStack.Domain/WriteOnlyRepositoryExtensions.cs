using System.Collections.Generic;

namespace EventStack.Domain
{
    public static class WriteOnlyRepositoryExtensions
    {
        public static void AddOrUpdateRange<TAggregate>(
            this IWriteOnlyRepository<TAggregate> repository,
            IEnumerable<TAggregate> range)
            where TAggregate : class, IAggregateRoot
        {
            foreach (var aggregate in range) repository.AddOrUpdate(aggregate);
        }
    }
}