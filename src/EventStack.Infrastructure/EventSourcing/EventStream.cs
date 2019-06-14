namespace EventStack.Infrastructure.EventSourcing
{
    public static class EventStream
    {
        public static EventStream<TEvent> Empty<TEvent>()
            where TEvent : class =>
            new EventStream<TEvent>();
    }
}