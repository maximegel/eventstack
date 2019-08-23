using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain
{
    public interface IReadOnlyRepository<TEntity, in TId>
        where TEntity : class, IEntity<TId>
    {
        Task<Option<TEntity>> FindAsync(TId id, CancellationToken cancellationToken = default);
    }
}