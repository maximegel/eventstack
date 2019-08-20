using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.Testing.EventSourcing.Doubles
{
    public class DummyFooUpdatedEvent : IDomainEvent
    {
        public DummyFooUpdatedEvent(string newValue) => Value = newValue;

        public string Value { get; }
    }
}