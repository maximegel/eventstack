using System.Threading;
using System.Threading.Tasks;

namespace EventStack.Infrastructure
{
    public class SingleParticipantUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWorkParticipant _participant;

        public SingleParticipantUnitOfWork(IUnitOfWorkParticipant participant) => _participant = participant;

        /// <inheritdoc />
        public Task CommitAsync(CancellationToken cancellationToken = default) =>
            _participant.SaveAsync(cancellationToken);
    }
}