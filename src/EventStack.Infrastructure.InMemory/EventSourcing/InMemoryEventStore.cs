using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStack.Domain.EventSourcing;
using EventStack.Infrastructure.EventSourcing;
using RailSharp;

namespace EventStack.Infrastructure.InMemory.EventSourcing
{
    /// <inheritdoc />
    public class InMemoryEventStore : IEventStore<EventDescriptor>
    {
        private const string CollectionKey = "_events";
        private readonly InMemoryStorage _storage;

        public InMemoryEventStore(InMemoryStorage storage) => _storage = storage;

        /// <inheritdoc />
        public Task AppendToStreamAsync(
            string streamId,
            IEnumerable<EventDescriptor> events,
            Option<long> expectedVersion,
            CancellationToken cancellationToken = default)
        {
            ControlConcurrency(streamId, expectedVersion);

            var storableEvents = events.Select(evnt => new InMemoryEventData(streamId, evnt));
            _storage.AddOrUpdateRange(CollectionKey, storableEvents);
            _storage.SaveChanges();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteStreamAsync(
            string streamId,
            Option<long> expectedVersion,
            CancellationToken cancellationToken = default)
        {
            ControlConcurrency(streamId, expectedVersion);

            _storage.RemoveRange(CollectionKey, ReadStoredStream(streamId));
            _storage.SaveChanges();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public IAsyncEnumerable<EventDescriptor> ReadStream(string streamId) =>
            ReadStoredStream(streamId)
                .Select(evnt => new EventDescriptor(evnt.SequenceNumber, evnt.Data))
                .ToAsyncEnumerable();

        private void ControlConcurrency(string streamId, Option<long> expectedVersion)
        {
            lock (_storage)
            {
                var storedStream = ReadStoredStream(streamId);
                var lastEvent = storedStream.TakeLast(1).ToList();
                if (lastEvent.Any(evnt => expectedVersion.Map(v => v != evnt.SequenceNumber).Reduce(false)))
                    throw new ConcurrencyException(
                        $"Expected version '{expectedVersion}', but found '{lastEvent.Single().SequenceNumber}'");
            }
        }

        private IEnumerable<InMemoryEventData> ReadStoredStream(string streamId) =>
            _storage.List<InMemoryEventData, Guid>(CollectionKey).Where(evnt => evnt.StreamId == streamId);
    }
}