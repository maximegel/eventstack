using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain
{
    public interface IWriteOnlyRepository<TAggregate, in TId>
        where TAggregate : class, IAggregateRoot<TId>
    {
        Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

        Task<Option<TAggregate>> FindAsync(TId id, CancellationToken cancellationToken = default);

        Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
    }
}