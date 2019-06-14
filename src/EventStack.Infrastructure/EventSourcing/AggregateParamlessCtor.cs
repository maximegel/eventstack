using EventStack.Domain;

namespace EventStack.Infrastructure.EventSourcing
{
    public class AggregateParamlessCtor<TAggregate> : IAggregateFactory<TAggregate>
        where TAggregate : IAggregateRoot, new()
    {
        /// <inheritdoc />
        public TAggregate Create() => new TAggregate();
    }
}