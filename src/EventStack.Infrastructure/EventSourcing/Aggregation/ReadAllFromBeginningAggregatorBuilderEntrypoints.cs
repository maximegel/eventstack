using EventStack.Common.Construction;
using EventStack.Domain;
using EventStack.Infrastructure.EventSourcing.Aggregation.Internal;

namespace EventStack.Infrastructure.EventSourcing.Aggregation
{
    public static class ReadAllFromBeginningAggregatorBuilderEntrypoints
    {
        public static IReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent>
            ReadAllFromBeginning<TAggregate, TEvent>(this BuilderExtensionPoint<IEventsAggregator<TAggregate, TEvent>> _)
            where TAggregate : IAggregateRoot<TAggregate, TEvent>
            where TEvent : class =>
            ReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent>.Initialize();
    }
}