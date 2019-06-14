using EventStack.Common.Construction;
using EventStack.Domain;
using EventStack.Infrastructure.EventSourcing.Aggregation;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepositoryBuilder<TAggregate, TEvent> :
        IBuildable<EventSourcedRepository<TAggregate, TEvent>>
        where TAggregate : class, IAggregateRoot<TAggregate, TEvent>
        where TEvent : class
    {
        private readonly IEventStore<TEvent> _eventStore;
        private IBuildable<IEventsAggregator<TAggregate, TEvent>> _aggregator;

        private EventSourcedRepositoryBuilder(IEventStore<TEvent> eventStore) => _eventStore = eventStore;

        public EventSourcedRepository<TAggregate, TEvent> Build() =>
            new EventSourcedRepository<TAggregate, TEvent>(_eventStore, _aggregator.Build());

        public static EventSourcedRepositoryBuilder<TAggregate, TEvent> For(IEventStore<TEvent> eventStore) =>
            new EventSourcedRepositoryBuilder<TAggregate, TEvent>(eventStore);

        public EventSourcedRepositoryBuilder<TAggregate, TEvent> UseAggregator(
            IEventsAggregator<TAggregate, TEvent> aggregator)
        {
            _aggregator = Builder.For(() => aggregator);
            return this;
        }

        public EventSourcedRepositoryBuilder<TAggregate, TEvent> UseAggregator(
            BuildingSteps<IEventsAggregator<TAggregate, TEvent>> buildingSteps)
        {
            _aggregator = buildingSteps();
            return this;
        }
    }
}