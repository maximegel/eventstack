using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Common;
using EventStack.Domain;
using EventStack.Infrastructure.EventSourcing.Aggregation;
using RailSharp;

namespace EventStack.Infrastructure.EventSourcing
{
    public class EventSourcedRepository<TAggregate, TEvent> : IWritableRepository<TAggregate>,
        IUnitOfWorkParticipant
        where TAggregate : class, IAggregateRoot<TAggregate, TEvent>
        where TEvent : class
    {
        private readonly IEventStore<TEvent> _eventStore;
        private readonly IEventsAggregator<TAggregate, TEvent> _aggregator;
        private readonly List<Func<Task>> _unsavedOperations = new List<Func<Task>>();

        internal EventSourcedRepository(
            IEventStore<TEvent> eventStore,
            IEventsAggregator<TAggregate, TEvent> aggregator)
        {
            _eventStore = eventStore;
            _aggregator = aggregator;
        }

        /// <inheritdoc />
        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            foreach (var operation in _unsavedOperations) await operation();
            await _eventStore.SaveAsync(cancellationToken);
        }

        public void AddOrUpdate(TAggregate aggregate) =>
            _unsavedOperations.Add(async () => await AddOrUpdateStreamAsync(aggregate));

        public void Remove(TAggregate aggregate) =>
            _unsavedOperations.Add(async () => await RemoveStreamAsync(aggregate));

        public async Task<Option<TAggregate>> TryFindAsync(object id, CancellationToken cancellationToken = default) =>
            await (await _eventStore.TryFindAsync(id, cancellationToken))
                .MapAsync(async stream => await _aggregator.ExecuteAsync(stream, cancellationToken));

        private async Task AddOrUpdateStreamAsync(TAggregate aggregate)
        {
            var stream = await GetOrCreateStreamAsync(aggregate);
            stream.Append(aggregate.Commit());
            _eventStore.AddOrUpdate(await GetOrCreateStreamAsync(aggregate));
        }

        private async Task<EventStream<TEvent>> GetOrCreateStreamAsync(TAggregate aggregate) =>
            (await _eventStore.TryFindAsync(aggregate.Id)).Reduce(EventStream.Empty<TEvent>());

        private async Task RemoveStreamAsync(TAggregate aggregate) =>
            (await _eventStore.TryFindAsync(aggregate.Id)).Do(stream => _eventStore.Remove(stream));
    }
}