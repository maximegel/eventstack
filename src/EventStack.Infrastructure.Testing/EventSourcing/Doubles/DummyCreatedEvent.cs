using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.Testing.EventSourcing.Doubles
{
    public class DummyCreatedEvent : IDomainEvent
    {
        public DummyCreatedEvent(int id) => Id = id;

        public int Id { get; }
    }
}