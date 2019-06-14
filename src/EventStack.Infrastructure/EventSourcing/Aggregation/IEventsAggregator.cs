using System.Threading;
using System.Threading.Tasks;

namespace EventStack.Infrastructure.EventSourcing.Aggregation
{
    public interface IEventsAggregator<TAggregate, TEvent>
        where TEvent : class
    {
        Task<TAggregate> ExecuteAsync(
            EventStream<TEvent> eventStream,
            CancellationToken cancellationToken = default);
    }
}