using System.Collections.Generic;
using System.Linq;
using RailSharp;

namespace EventStack.Domain.EventSourcing
{
    public abstract class EventSourcedAggregateRoot<TId> : Entity<TId>,
        IAggregateRoot<TId>,
        IEventSource
    {
        private readonly ICollection<(IDomainEvent evnt, long version)> _uncommitedEvents =
            new List<(IDomainEvent evnt, long version)>();

        private long _version;

        protected EventSourcedAggregateRoot() { }

        protected EventSourcedAggregateRoot(TId id)
            : base(id)
        {
        }

        /// <inheritdoc />
        IEventSource IEventSource.Apply(EventDescriptor @event)
        {
            Apply(@event.Data);
            _version = @event.Version;
            return this;
        }

        /// <inheritdoc />
        IReadOnlyCollection<EventDescriptor> IEventSource.Commit()
        {
            var events = _uncommitedEvents.Select(pair => new EventDescriptor(pair.version, pair.evnt)).ToList();
            _uncommitedEvents.Clear();
            return events;
        }

        protected abstract void Apply(IDomainEvent @event);

        protected void Emit(IDomainEvent @event)
        {
            Apply(@event);
            _uncommitedEvents.Add((@event, ++_version));
        }
    }
}