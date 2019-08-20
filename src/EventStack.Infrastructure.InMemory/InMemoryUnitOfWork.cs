using System.Threading;
using System.Threading.Tasks;

namespace EventStack.Infrastructure.InMemory
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly InMemoryStorage _storage;

        public InMemoryUnitOfWork(InMemoryStorage storage) => _storage = storage;

        /// <inheritdoc />
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            _storage.SaveChanges();
            return Task.CompletedTask;
        }
    }
}