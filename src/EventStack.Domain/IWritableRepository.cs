using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Domain
{
    public interface IWritableRepository<TEntity>
        where TEntity : class
    {
        void AddOrUpdate(TEntity entity);

        void Remove(TEntity entity);

        Task<Option<TEntity>> TryFindAsync(object id, CancellationToken cancellationToken = default);
    }
}