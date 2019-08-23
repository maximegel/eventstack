using System;
using EventStack.Domain.EventSourcing;
using EventStack.Infrastructure.Testing.Doubles;

namespace EventStack.Infrastructure.Testing.EventSourcing.Doubles
{
    public class DummyEventSourcedAggregateRoot : EventSourcedAggregateRoot<int>,
        IDummyAggregateRoot
    {
        public DummyEventSourcedAggregateRoot(int id)
            : base(id) => Emit(new DummyCreatedEvent(id));

        private DummyEventSourcedAggregateRoot() { }

        /// <inheritdoc />
        public string Foo { get; private set; }

        /// <inheritdoc />
        public void UpdateFoo(string value) => Emit(new DummyFooUpdatedEvent(value));

        /// <inheritdoc />
        protected override void Apply(IDomainEvent @event)
        {
            switch (@event)
            {
                case DummyFooUpdatedEvent evnt:
                    Foo = evnt.Value;
                    return;
                case DummyCreatedEvent evnt:
                    Id = evnt.Id;
                    return;
                default: throw new ArgumentException("Unhandled event type.", nameof(@event));
            }
        }
    }
}