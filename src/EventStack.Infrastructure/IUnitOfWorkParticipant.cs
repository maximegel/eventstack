using System.Threading;
using System.Threading.Tasks;

namespace EventStack.Infrastructure
{
    public interface IUnitOfWorkParticipant
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}