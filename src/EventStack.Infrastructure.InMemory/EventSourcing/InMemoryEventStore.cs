using EventStack.Infrastructure.EventSourcing;
using RailSharp;

namespace EventStack.Infrastructure.InMemory.EventSourcing
{
    /// <inheritdoc />
    public class InMemoryEventStore<TEvent> : IEventStore<TEvent>
        where TEvent : class
    {
        private readonly object _collectionKey = typeof(InMemoryEventStore<>);
        private readonly InMemoryStorage _storage;

        public InMemoryEventStore(InMemoryStorage storage) => _storage = storage;

        /// <inheritdoc />
        public void AddOrUpdate(IEventStream<TEvent> stream) =>
            _storage.AddOrUpdate(_collectionKey, stream);

        /// <inheritdoc />
        public IEventStream<TEvent> Stream(object id) =>
            _storage.TryFind<IEventStream<TEvent>>(_collectionKey, id).Reduce(new InMemoryEventStream<TEvent>(id));
    }
}