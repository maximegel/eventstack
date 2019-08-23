using EventStack.Domain;
using EventStack.Domain.EventSourcing;

namespace EventStack.Infrastructure.EventSourcing
{
    public static class EventSourcedRepositoryBuilder
    {
        public static EventSourcedRepositoryBuilder<TAggregate, TId>.IInitialized For<TAggregate, TId>()
            where TAggregate : class, IAggregateRoot<TId>, IEventSource =>
            EventSourcedRepositoryBuilder<TAggregate, TId>.Initialize();
    }
}