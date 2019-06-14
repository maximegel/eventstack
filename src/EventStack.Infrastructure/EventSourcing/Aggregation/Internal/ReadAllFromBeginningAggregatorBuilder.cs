using EventStack.Common.Construction;
using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing.Aggregation.Internal
{
    internal class ReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent> :
        IReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent>,
        IBuildable<IEventsAggregator<TAggregate, TEvent>>
        where TAggregate : IAggregateRoot<TAggregate, TEvent>
        where TEvent : class
    {
        private IAggregateFactory<TAggregate> _aggregateFactory;

        private ReadAllFromBeginningAggregatorBuilder() { }

        /// <inheritdoc />
        public IEventsAggregator<TAggregate, TEvent> Build() =>
            new ReadAllFromBeginningAggregator<TAggregate, TEvent>(_aggregateFactory);

        public IBuildable<IEventsAggregator<TAggregate, TEvent>> WithAggregateFactory(
            IAggregateFactory<TAggregate> aggregateFactory)
        {
            _aggregateFactory = aggregateFactory;
            return this;
        }

        public static IReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent> Initialize() =>
            new ReadAllFromBeginningAggregatorBuilder<TAggregate, TEvent>();
    }
}