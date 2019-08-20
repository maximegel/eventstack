namespace EventStack.Infrastructure.EventSourcing
{
    public static class EventStreamExtensions
    {
        public static IEventStream<TEvent> Append<TEvent>(this IEventStream<TEvent> stream, TEvent @event)
            where TEvent : class =>
            stream.Append(new[] {@event});
    }
}