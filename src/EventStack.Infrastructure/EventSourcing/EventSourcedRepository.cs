using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Domain.EventSourcing;
using EventStack.Infrastructure.EventSourcing.Internal;
using RailSharp;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepository<TAggregate> : IWriteOnlyRepository<TAggregate>
        where TAggregate : class, IAggregateRoot, IEventSource
    {
        private readonly IAggregateFactory<TAggregate> _aggregateFactory;
        private readonly IEventStore<IDomainEvent> _eventStore;

        internal EventSourcedRepository(
            IEventStore<IDomainEvent> eventStore,
            IAggregateFactory<TAggregate> aggregateFactory)
        {
            _eventStore = eventStore;
            _aggregateFactory = aggregateFactory;
        }

        public void AddOrUpdate(TAggregate aggregate) =>
            aggregate.Commit(events => _eventStore.AddOrUpdate(_eventStore.Stream(aggregate.Id).Append(events)));

        public void Remove(TAggregate aggregate) =>
            aggregate.Commit(
                events => _eventStore.AddOrUpdate(
                    _eventStore.Stream(aggregate.Id).Append(new AggregateRemovedEvent())));

        public async Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken = default) =>
            await _eventStore.Stream(id)
                .AggregateAsync(
                    (Option<TAggregate>) Option.None,
                    (aggregate, @event) =>
                    {
                        switch (@event)
                        {
                            case AggregateRemovedEvent _: return Option.None;
                            default: return (TAggregate) aggregate.Reduce(_aggregateFactory.Create).Apply(@event);
                        }
                    });
    }
}