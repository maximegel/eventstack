using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain
{
    public interface IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default);
    }
}