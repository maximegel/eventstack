using EventStack.Domain;
using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepositoryBuilder<TAggregate>
        where TAggregate : class, IAggregateRoot, IEventSource
    {
        private readonly IEventStore<IDomainEvent> _eventStore;

        private IAggregateFactory<TAggregate> _aggregateFactory =
            AggregateFactories.NonPublicParamlessCtor<TAggregate>();

        private EventSourcedRepositoryBuilder(IEventStore<IDomainEvent> eventStore) => _eventStore = eventStore;

        public static EventSourcedRepositoryBuilder<TAggregate> For(IEventStore<IDomainEvent> eventStore) =>
            new EventSourcedRepositoryBuilder<TAggregate>(eventStore);

        public EventSourcedRepository<TAggregate> Build() =>
            new EventSourcedRepository<TAggregate>(_eventStore, _aggregateFactory);

        public EventSourcedRepositoryBuilder<TAggregate> UseAggregateFactory(
            IAggregateFactory<TAggregate> aggregateFactory)
        {
            _aggregateFactory = aggregateFactory;
            return this;
        }
    }
}