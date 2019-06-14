using EventStack.Common.Construction;
using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing.Aggregation.Internal
{
    public interface IReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent>
        where TAggregate : IAggregateRoot
        where TEvent : class
    {
        IBuildable<IEventsAggregator<TAggregate, TEvent>> WithAggregateFactory(
            IAggregateFactory<TAggregate> aggregateFactory);
    }
}