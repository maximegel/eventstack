namespace EventStack.Domain.Internal
{
    internal class AggregateParamlessCtor<TAggregate> : IAggregateFactory<TAggregate>
        where TAggregate : IAggregateRoot, new()
    {
        /// <inheritdoc />
        public TAggregate Create() => new TAggregate();
    }
}