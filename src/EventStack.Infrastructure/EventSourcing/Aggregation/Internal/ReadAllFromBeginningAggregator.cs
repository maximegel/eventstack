using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing.Aggregation.Internal
{
    internal class ReadAllFromBeginningAggregator<TAggregate, TEvent> : IEventsAggregator<TAggregate, TEvent>
        where TAggregate : IAggregateRoot<TAggregate, TEvent>
        where TEvent : class
    {
        private readonly IAggregateFactory<TAggregate> _aggregateFactory;

        public ReadAllFromBeginningAggregator(IAggregateFactory<TAggregate> aggregateFactory) =>
            _aggregateFactory = aggregateFactory;

        public async Task<TAggregate> ExecuteAsync(
            EventStream<TEvent> eventStream,
            CancellationToken cancellationToken = default) =>
            await eventStream.ReadForward()
                .Aggregate(
                    _aggregateFactory.Create(),
                    (aggregate, @event) => aggregate.Apply(@event),
                    cancellationToken);
    }
}