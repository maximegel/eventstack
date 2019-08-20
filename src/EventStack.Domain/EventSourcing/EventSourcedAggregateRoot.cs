using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStack.Domain.EventSourcing
{
    public abstract class EventSourcedAggregateRoot<TId> : Entity<TId>,
        IAggregateRoot,
        IEventSource,
        IVersioned
    {
        private readonly ICollection<IDomainEvent> _uncommitedEvents = new List<IDomainEvent>();

        protected EventSourcedAggregateRoot() { }

        protected EventSourcedAggregateRoot(TId id)
            : base(id)
        {
        }

        /// <inheritdoc />
        IEventSource IEventSource.Apply(IDomainEvent @event)
        {
            Apply(@event);
            Version++;
            return this;
        }

        /// <inheritdoc />
        IEventSource IEventSource.Commit(Action<IEnumerable<IDomainEvent>> handler)
        {
            handler(_uncommitedEvents.ToList());
            _uncommitedEvents.Clear();
            return this;
        }

        /// <inheritdoc />
        public long Version { get; private set; }

        protected abstract void Apply(IDomainEvent @event);

        protected void Raise(IDomainEvent @event)
        {
            Apply(@event);
            _uncommitedEvents.Add(@event);
            Version++;
        }
    }
}