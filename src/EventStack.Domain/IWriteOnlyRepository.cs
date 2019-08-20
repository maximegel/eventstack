using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain
{
    public interface IWriteOnlyRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
    {
        Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken = default);

        void AddOrUpdate(TAggregate aggregate);

        void Remove(TAggregate aggregate);
    }
}