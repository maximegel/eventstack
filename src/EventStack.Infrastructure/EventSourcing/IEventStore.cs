namespace EventStack.Infrastructure.EventSourcing
{
    public interface IEventStore<TEvent>
        where TEvent : class
    {
        void AddOrUpdate(IEventStream<TEvent> stream);

        IEventStream<TEvent> Stream(object id);
    }
}