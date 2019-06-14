using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing
{
    public interface IEventStore<TEvent> :
        IWritableRepository<EventStream<TEvent>>,
        IUnitOfWorkParticipant
        where TEvent : class
    {
    }
}