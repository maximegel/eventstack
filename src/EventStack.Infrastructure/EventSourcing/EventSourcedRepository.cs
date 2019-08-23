using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Domain.EventSourcing;
using RailSharp;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepository<TAggregate, TId> : IWriteOnlyRepository<TAggregate, TId>
        where TAggregate : class, IAggregateRoot<TId>, IEventSource
    {
        internal Func<TAggregate> AggregateFactory;
        internal IEventStore<EventDescriptor> EventStore;
        internal Func<TId, Type, string> StreamIdResolver;

        internal EventSourcedRepository() { }

        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default) =>
            EventStore.DeleteStreamAsync(StreamId(aggregate.Id), Option.None, cancellationToken);

        public async Task<Option<TAggregate>> FindAsync(TId id, CancellationToken cancellationToken = default) =>
            await EventStore.ReadStream(StreamId(id))
                .Aggregate(
                    (Option<TAggregate>) Option.None,
                    (aggregate, @event) => (TAggregate) aggregate.Reduce(AggregateFactory()).Apply(@event),
                    cancellationToken);

        public Task SaveAsync(TAggregate aggregate, CancellationToken cancellationToken = default) =>
            EventStore.AppendToStreamAsync(
                StreamId(aggregate.Id),
                aggregate.Commit(),
                Option.None,
                cancellationToken);

        private string StreamId(TId aggregateId) => StreamIdResolver(aggregateId, typeof(TAggregate));
    }
}