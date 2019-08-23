using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RailSharp;

namespace EventStack.Infrastructure.EventSourcing
{
    public interface IEventStore<TEvent>
        where TEvent : class
    {
        Task AppendToStreamAsync(
            string streamId,
            IEnumerable<TEvent> events,
            Option<long> expectedVersion,
            CancellationToken cancellationToken = default);

        Task DeleteStreamAsync(
            string streamId,
            Option<long> expectedVersion,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<TEvent> ReadStream(string streamId);
    }
}