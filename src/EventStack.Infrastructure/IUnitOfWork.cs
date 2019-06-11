using System.Threading;
using System.Threading.Tasks;

namespace EventStack.Infrastructure
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}