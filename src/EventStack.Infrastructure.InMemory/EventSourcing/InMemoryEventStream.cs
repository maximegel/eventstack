using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStack.Domain;
using EventStack.Infrastructure.EventSourcing;

namespace EventStack.Infrastructure.InMemory.EventSourcing
{
    /// <inheritdoc cref="IEventStream{TEvent}" />
    public class InMemoryEventStream<TEvent> : Entity<object>,
        IEventStream<TEvent>
        where TEvent : class
    {
        private readonly IEnumerable<TEvent> _events;

        /// <inheritdoc />
        public InMemoryEventStream(object id)
            : this(id, Enumerable.Empty<TEvent>())
        {
        }

        public InMemoryEventStream(object id, IEnumerable<TEvent> events)
            : base(id) => _events = events;

        /// <inheritdoc />
        public Task<TAccumulate> AggregateAsync<TAccumulate>(
            TAccumulate seed,
            Func<TAccumulate, TEvent, TAccumulate> func) => Task.FromResult(_events.Aggregate(seed, func));

        /// <inheritdoc />
        public IEventStream<TEvent> Append(IEnumerable<TEvent> events) =>
            new InMemoryEventStream<TEvent>(Id, _events.Concat(events));
    }
}